// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Models;
using Educ8IT.AspNetCore.SimpleApi.RouteContraints;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class VersionRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiMapperOptions _apiMapperOptions;

        /// <summary>
        /// DI here is Singleton type - app lifetime
        /// </summary>
        /// <param name="next"></param>
        /// <param name="apiMapperOptions"></param>
        public VersionRoutingMiddleware(RequestDelegate next, IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
        {
            _next = next;
            _apiMapperOptions = apiMapperOptions?.CurrentValue ?? throw new ArgumentNullException(nameof(apiMapperOptions));
        }

        /// <summary>
        /// DI here is scoped
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Don't do anything if Versioning is turned off
            if (!_apiMapperOptions.UseVersioning)
            {
                await _next(context);
                return;
            }

            // Get endpoint, or skip to next Middleware
            var __endpoint = context.GetEndpoint() as RouteEndpoint;
            if (__endpoint == null)
            {
                await _next(context);
                return;
            }

            // Obtain Controlller + Action, or skip to next Middleware
            var __methodItem = __endpoint.Metadata.GetMetadata<ApiMethodItem>();
            if (__methodItem == null || __methodItem.ApiControllerItem == null)
            {
                await _next(context);
                return;
            }

            // Check if Controller + Action indicate any versions
            List<ApiVersion> __versionsAllowed = new List<ApiVersion>();
            if (__methodItem.ApiControllerItem.Versions != null && __methodItem.ApiControllerItem.Versions.Count > 0)
            {
                __versionsAllowed.AddRange(__methodItem.ApiControllerItem.Versions);
            }
            if (__methodItem.Versions != null && __methodItem.Versions.Count > 0)
            {
                __versionsAllowed.AddRange(__methodItem.Versions);
            }

            // Check if we should use Default ApiVersion
            if (__versionsAllowed.Count == 0)
            {
                if (_apiMapperOptions.AssumeDefaultVersionWhenUnspecified)
                {
                    if (_apiMapperOptions.DefaultApiVersion != null)
                        __versionsAllowed.Add(_apiMapperOptions.DefaultApiVersion);
                }
            }

            // If no version information specified, skip to next Middleware
            if (__versionsAllowed.Count == 0)
            {
                await _next(context);
                return;
            }
            else if (_apiMapperOptions.ReportApiVersions)
            {
                // Report Version supported by the Endpoint
                StringValues sv = new StringValues(__versionsAllowed.Select(v => v.VersionText).ToArray());
                context.Response.Headers.Add("api-supported-versions", sv);
            }

            // Obtain version info from Header, Route, QueryString
            ApiVersion apiVersion = null;

            ApiVersionParseOutcome headerVersion = context.GetHeaderVersion(_apiMapperOptions.HeaderApiVersionKey ?? "api-version");
            ApiVersionParseOutcome routeVersion = context.GetRouteVersion(__endpoint);
            ApiVersionParseOutcome queryVersion = context.GetQueryStringVersion(_apiMapperOptions.QueryApiVersionKey ?? "api-version");

            if (headerVersion.Outcome == EApiVersionParseOutcome.Matched)
            {
                apiVersion = headerVersion.ApiVersion;
            }
            else if (headerVersion.ValueIsFaulty)
            {
                var __problemDetails = headerVersion.GetProblemDetails(__versionsAllowed);
                await context.SendProblemDetailsResponse(__problemDetails);
                return;
            }

            if (routeVersion.Outcome == EApiVersionParseOutcome.Matched)
            {
                if (_apiMapperOptions.AllowVersionOverrides)
                {
                    apiVersion = routeVersion.ApiVersion;
                }
                else if (apiVersion != null && !apiVersion.Equals(routeVersion.ApiVersion))
                {
                    routeVersion.Outcome = EApiVersionParseOutcome.InconsistentVersion;
                    var __problemDetails = routeVersion.GetProblemDetails(__versionsAllowed);
                    __problemDetails.AddItem("previousResults",
                        $"Parsed from {headerVersion.Source ?? "Unknown"}. " +
                        $"Version requested = {headerVersion.ApiVersion?.ToString() ?? "Unknown"}");

                    await context.SendProblemDetailsResponse(__problemDetails);
                    return;
                }
                else
                {
                    apiVersion = routeVersion.ApiVersion;
                }
            }
            else if (routeVersion.ValueIsFaulty)
            {
                var __problemDetails = routeVersion.GetProblemDetails(__versionsAllowed);
                await context.SendProblemDetailsResponse(__problemDetails);
                return;
            }

            if (queryVersion.Outcome == EApiVersionParseOutcome.Matched)
            {
                if (_apiMapperOptions.AllowVersionOverrides)
                {
                    apiVersion = queryVersion.ApiVersion;
                }
                else if (apiVersion != null && !apiVersion.Equals(queryVersion.ApiVersion))
                {
                    queryVersion.Outcome = EApiVersionParseOutcome.InconsistentVersion;
                    var __problemDetails = queryVersion.GetProblemDetails(__versionsAllowed);
                    __problemDetails.AddItem("previousResults",
                        $"Parsed from {headerVersion.Source ?? "Unknown"}. " +
                        $"Version requested = {headerVersion.ApiVersion?.ToString() ?? "Unknown"}",
                        $"Parsed from {routeVersion.Source ?? "Unknown"}. " +
                        $"Version requested = {routeVersion.ApiVersion?.ToString() ?? "Unknown"}");

                    await context.SendProblemDetailsResponse(__problemDetails);
                    return;
                }
                else
                {
                    apiVersion = queryVersion.ApiVersion;
                }
            }
            else if (queryVersion.ValueIsFaulty)
            {
                var __problemDetails = queryVersion.GetProblemDetails(__versionsAllowed);
                await context.SendProblemDetailsResponse(__problemDetails);
                return;
            }

            // If one (or more) versions were requested...
            if (apiVersion != null)
            {
                context.Items.Add("ApiVersion", apiVersion);

                // If a version was requested via Query, check it is supported on the Method/Action
                if (queryVersion.Outcome == EApiVersionParseOutcome.Matched)
                {
                    queryVersion.Outcome = CheckApiVersions(queryVersion.ApiVersion, __versionsAllowed);
                    if (queryVersion.Outcome == EApiVersionParseOutcome.Matched)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        var __problemDetails = queryVersion.GetProblemDetails(__versionsAllowed);
                        await context.SendProblemDetailsResponse(__problemDetails);
                        return;
                    }
                }

                // If a version was requested via Route, check it is supported on the Method/Action
                if (routeVersion.Outcome == EApiVersionParseOutcome.Matched)
                {
                    routeVersion.Outcome = CheckApiVersions(routeVersion.ApiVersion, __versionsAllowed);
                    if (routeVersion.Outcome == EApiVersionParseOutcome.Matched)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        var __problemDetails = routeVersion.GetProblemDetails(__versionsAllowed);
                        await context.SendProblemDetailsResponse(__problemDetails);
                        return;

                    }
                }

                // If a version was requested via Header, check it is supported on the Method/Action
                if (headerVersion.Outcome == EApiVersionParseOutcome.Matched)
                {
                    headerVersion.Outcome = CheckApiVersions(headerVersion.ApiVersion, __versionsAllowed);
                    if (headerVersion.Outcome == EApiVersionParseOutcome.Matched)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        var __problemDetails = headerVersion.GetProblemDetails(__versionsAllowed);
                        await context.SendProblemDetailsResponse(__problemDetails);
                        return;
                    }
                }
            }
            else if (_apiMapperOptions.AssumeDefaultVersionWhenUnspecified && _apiMapperOptions.DefaultApiVersion != null)
            {
                apiVersion = _apiMapperOptions.DefaultApiVersion;
                context.Items.Add("api-version", apiVersion);

                await _next(context);
            }
            else
            {
                var __problemDetails = new ProblemDetails()
                {
                    Detail = $"No version specified and it is required",
                    Status = 422,
                    Title = "API Version unspecified"
                };
                await context.SendProblemDetailsResponse(__problemDetails);
                return;
            }
        }

        /// <summary>
        /// Check if the requested ApiVersion is supported by the method
        /// </summary>
        /// <param name="requestedVersion"></param>
        /// <param name="versionsOnMethod"></param>
        /// <returns></returns>
        private EApiVersionParseOutcome CheckApiVersions(ApiVersion requestedVersion, List<ApiVersion> versionsOnMethod)
        {
            if (requestedVersion == null)
                return EApiVersionParseOutcome.Missing;

            if (versionsOnMethod == null)
                return EApiVersionParseOutcome.UnsupportedVersionOnMethod;

            foreach (var __versionAllowed in versionsOnMethod)
            {
                if (requestedVersion.HasBuild || requestedVersion.HasRevision)
                {
                    if (__versionAllowed.Equals(requestedVersion))
                    {
                        return EApiVersionParseOutcome.Matched;
                    }
                }
                else
                {
                    if (__versionAllowed.EqualsSameMajorAndMinor(requestedVersion))
                    {
                        return EApiVersionParseOutcome.Matched;
                    }
                }
            }

            return EApiVersionParseOutcome.UnsupportedVersionOnMethod;
        }
    }

    /// <summary>
    /// Parse result for a source (ie Header, Routem, Query). 
    /// If successfully parsed, Outcome will be Matched and ApiVersion will contain the successfully parsed version. 
    /// ... we do not store the value that we attempted to parse so we can send it back.
    /// At the end of the day, that should be obvious by inspecting the request and it could lead to 
    ///     injection attacks.
    /// </summary>
    public class ApiVersionParseOutcome
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiVersionParseOutcome() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public ApiVersionParseOutcome(string source)
            :this()
        {
            Source = source;
        }

        /// <summary>
        /// Header, Route, Query or other source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion ApiVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EApiVersionParseOutcome Outcome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ValueIsFaulty
        {
            get
            {
                return Outcome == EApiVersionParseOutcome.AmbigousVersion
                        || Outcome == EApiVersionParseOutcome.MalformedVersion;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Outcome = {Outcome}; Version = {ApiVersion?.ToString() ?? "Unknown"}; IsFaulty: {ValueIsFaulty}";
            //return base.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EApiVersionParseOutcome
    {
        /// <summary>
        /// Version was found and parsed
        /// </summary>
        Matched,

        /// <summary>
        /// No version specified
        /// </summary>
        Missing,

        /// <summary>
        /// The Method/Action does not support this version
        /// </summary>
        UnsupportedVersionOnMethod,

        /// <summary>
        /// No Method/Action supports this version (not really used)
        /// </summary>
        UnsupportedMethodOnVersion,

        /// <summary>
        /// Unable to parse the requested version
        /// </summary>
        MalformedVersion,

        /// <summary>
        /// Multiple different versions have been requested in the source (Header, Route, Query)
        /// </summary>
        AmbigousVersion,

        /// <summary>
        /// Multiple different versions have been requested across all sources.
        /// Use <see cref="ApiMapperOptions.AllowVersionOverrides"/> to examine for this.
        /// Normally we Query to override Route to override Header.
        /// </summary>
        InconsistentVersion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class VersionRoutingMiddlewareExtensions
    {
        /// <summary>
        /// This should be run after UseAuth and UseRouting but before UseEndpoints
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseVersionRouting(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VersionRoutingMiddleware>();
        }

        /// <summary>
        /// Get the ApiVersion from a Header keyed by headerKey
        /// </summary>
        /// <param name="context">Current Http Context</param>
        /// <param name="headerKey">Header key. Default value is "api-version"</param>
        /// <returns>An ApiVersionParseOutcome object</returns>
        public static ApiVersionParseOutcome GetHeaderVersion(this HttpContext context, string headerKey = "api-version")
        {
            ApiVersionParseOutcome apiVersionParseOutcome = new ApiVersionParseOutcome("header");

            var __headerKeyName = headerKey ?? "api-version";

            if (!context.Request.Headers.ContainsKey(__headerKeyName))
            {
                apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
            }
            else
            {
                var __headerValues = context.Request.Headers[__headerKeyName];
                if (__headerValues.Count == 0)
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
                }
                else if (__headerValues.Count > 1)
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.AmbigousVersion;
                }
                else
                {
                    string __headerVersion = __headerValues.ToString();
                    if (String.IsNullOrEmpty(__headerVersion))
                    {
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
                    }
                    else if (__headerVersion != null && __headerVersion.Contains(","))
                    {
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.AmbigousVersion;
                    }
                    else if (ApiVersion.TryParse(__headerVersion, out ApiVersion apiVersion))
                    {
                        apiVersionParseOutcome.ApiVersion = apiVersion;
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Matched;
                    }
                    else
                    {
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.MalformedVersion;
                    }
                }
            }

            return apiVersionParseOutcome;
        }

        /// <summary>
        /// Get the ApiVersion from the route when route contraint is ":apiVersion" or 
        ///     whatever <see cref="VersionRouteConstraint.RouteConstraintKey"/> defines.
        /// </summary>
        /// <param name="context">Current Http Context</param>
        /// <param name="endpoint">The matched Route Endpoint</param>
        /// <returns>An ApiVersionParseOutcome object</returns>
        public static ApiVersionParseOutcome GetRouteVersion(this HttpContext context, RouteEndpoint endpoint)
        {
            ApiVersionParseOutcome apiVersionParseOutcome = new ApiVersionParseOutcome("route");

            List<string> __routeVersions = new List<string>();
            
            if (endpoint.RoutePattern.Parameters != null)
            {
                foreach (var param in endpoint.RoutePattern.Parameters)
                {
                    if (param.ParameterPolicies != null)
                    {
                        foreach (var policy in param.ParameterPolicies)
                        {
                            if (policy.Content == VersionRouteConstraint.RouteConstraintKey)
                            {
                                __routeVersions.Add(context.GetRouteValue(param.Name) as string);
                            }
                        }
                    }
                }
            }

            if (__routeVersions.Count == 0)
            {
                apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
            }
            else if (__routeVersions.Count > 1)
            {
                apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.AmbigousVersion;
            }
            else
            {
                var __routeVersion = __routeVersions.FirstOrDefault();

                if (String.IsNullOrEmpty(__routeVersion))
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
                }
                else if (ApiVersion.TryParse(__routeVersion, out ApiVersion apiVersion))
                {
                    apiVersionParseOutcome.ApiVersion = apiVersion;
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Matched;
                }
                else
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.MalformedVersion;
                }
            }

            return apiVersionParseOutcome;
        }

        /// <summary>
        /// Get the ApiVersion from the QueryString keyed by queryKey
        /// </summary>
        /// <param name="context">Current Http Context</param>
        /// <param name="queryKey">QueryString key. Default value is "api-version"</param>
        /// <returns>An ApiVersionParseOutcome object</returns>
        public static ApiVersionParseOutcome GetQueryStringVersion(this HttpContext context, string queryKey = "api-version")
        {
            ApiVersionParseOutcome apiVersionParseOutcome = new ApiVersionParseOutcome("query");

            var __queryKeyName = queryKey ?? "api-version";

            if (!context.Request.Query.ContainsKey(__queryKeyName))
            {
                apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
            }
            else
            {
                var __queryValues = context.Request.Query[__queryKeyName];
                if (__queryValues.Count == 0)
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
                }
                else if (__queryValues.Count > 1)
                {
                    apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.AmbigousVersion;
                }
                else
                {
                    string __queryVersion = __queryValues.ToString();
                    if (String.IsNullOrEmpty(__queryVersion))
                    {
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Missing;
                    }
                    else if (ApiVersion.TryParse(__queryVersion, out ApiVersion apiVersion))
                    {
                        apiVersionParseOutcome.ApiVersion = apiVersion;
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.Matched;
                    }
                    else
                    {
                        apiVersionParseOutcome.Outcome = EApiVersionParseOutcome.MalformedVersion;
                    }
                }
            }

            return apiVersionParseOutcome;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="problemDetails"></param>
        /// <returns></returns>
        public static async Task SendProblemDetailsResponse(this HttpContext context, ProblemDetails problemDetails)
        {
            var __problemDetailsAsString = JsonConvert.SerializeObject(problemDetails);

            context.Response.StatusCode = problemDetails.Status;
            context.Response.ContentType = "application/problem+json";
            context.Response.ContentLength = __problemDetailsAsString.Length;

            await context.Response.WriteAsync(__problemDetailsAsString);
        }


        /// <summary>
        /// Return a new Problem Details object for the ApiVersionParseOutcome
        /// </summary>
        /// <param name="apiVersionParseOutcome"></param>
        /// <param name="versionsOnMethod"></param>
        /// <returns></returns>
        public static ProblemDetails GetProblemDetails(this ApiVersionParseOutcome apiVersionParseOutcome, List<ApiVersion> versionsOnMethod)
        {
            ProblemDetails problemDetails = null;
            switch (apiVersionParseOutcome.Outcome)
            {
                case EApiVersionParseOutcome.AmbigousVersion:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}" +
                            $". Version requested = {apiVersionParseOutcome.ApiVersion?.ToString() ?? "Unknown"}",
                        Status = 400,
                        Title = "Ambigous API Version requested"
                    };
                    break;
                case EApiVersionParseOutcome.InconsistentVersion:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}" +
                            $". Version requested = {apiVersionParseOutcome.ApiVersion?.ToString() ?? "Unknown"}",
                        Status = 400,
                        Title = "Inconsistent API Version requested"
                    };
                    break;

                case EApiVersionParseOutcome.MalformedVersion:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}",
                        Status = 400,
                        Title = "Malformed API Version requested"
                    };
                    break;

                case EApiVersionParseOutcome.Matched:
                    break;

                case EApiVersionParseOutcome.Missing:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}",
                        Status = 400,
                        Title = "API Version unspecified"
                    };
                    break;

                case EApiVersionParseOutcome.UnsupportedMethodOnVersion:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}" +
                            $". Version requested = {apiVersionParseOutcome.ApiVersion?.ToString() ?? "Unknown"}",
                        Status = 405,
                        Title = "Method/Action not supported by API Version requested"
                    };
                    break;
                case EApiVersionParseOutcome.UnsupportedVersionOnMethod:
                    problemDetails = new ProblemDetails()
                    {
                        Detail = $"Parsed from {apiVersionParseOutcome.Source ?? "Unknown"}" +
                            $". Version requested = {apiVersionParseOutcome.ApiVersion?.ToString() ?? "Unknown"}",
                        Status = 400,
                        Title = "Method/Action does not support API Version requested"
                    };
                    break;
            }
            problemDetails.AddItem("supportedVersions", versionsOnMethod.Select(v => v.ToString()).ToArray());

            return problemDetails;
        }
    }
}
