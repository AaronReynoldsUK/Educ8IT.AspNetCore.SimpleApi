// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Educ8IT.AspNetCore.SimpleApi.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomEndpointDataSource : EndpointDataSource, IEndpointConventionBuilder
    {
        private readonly ILoggerFactory _iLoggerFactory;
        private readonly RoutePatternTransformer _routePatternTransformer;
        private readonly ApiMapperService _apiMapperService;
        private readonly List<Action<EndpointBuilder>> _conventions;
        private readonly List<string> _endpointNames;

        //public List<RoutePattern> Patterns { get; }
        //public List<HubMethod> HubMethods { get; }

        private List<Endpoint> _endpoints;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routePatternTransformer"></param>
        /// <param name="iLoggerFactory"></param>
        /// <param name="apiMapperService"></param>
        public CustomEndpointDataSource(
            RoutePatternTransformer routePatternTransformer,
            ILoggerFactory iLoggerFactory,
            IApiMapperService apiMapperService
            )
        {
            _iLoggerFactory = iLoggerFactory ?? throw new ArgumentNullException(nameof(iLoggerFactory));
            _routePatternTransformer = routePatternTransformer;
            _apiMapperService = (ApiMapperService)apiMapperService;
            _conventions = new List<Action<EndpointBuilder>>();
            _endpointNames = new List<string>();

            //Patterns = new List<RoutePattern>();
            //HubMethods = new List<HubMethod>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override IReadOnlyList<Endpoint> Endpoints
        {
            get
            {
                if (_endpoints == null)
                {
                    _endpoints = BuildEndpoints();
                }

                return _endpoints;
            }
        }

        private List<Endpoint> BuildEndpoints()
        {
            List<Endpoint> endpoints = new List<Endpoint>();

            foreach (var __controller in _apiMapperService.ApiDescription.Controllers)
            {
                foreach (var __method in __controller.Methods)
                {
                    var __defaultValues = new
                    {
                        controller = __controller.Name,
                        action = __method.Name,
                        method = __method.Name
                    };
                    //var __placeHolderValues = new
                    //{
                    //    controller = __controller.Name,
                    //    action = __method.Name,
                    //    method = __method.Name
                    //};
                    var order = 1;
                    foreach (var __route in __method.ActionRoutes)
                    {
                        if (__controller.RoutePrefixes != null)
                        {
                            foreach (var __routePrefix in __controller.RoutePrefixes)
                            {
                                // Do this to stop Conventions being added to all subsequent routes
                                _conventions.Clear();

                                // Combine Patterns
                                var __routePattern = __route.GetRoutePattern(
                                    __routePrefix,
                                    __defaultValues,
                                    null,
                                    null);

                                // won't ever match as these as REQUIRED values
                                var __resolvedPattern = _routePatternTransformer.SubstituteRequiredValues(
                                    __routePattern, null);

                                //TEST
                                __resolvedPattern ??= __routePattern;

                                if (__resolvedPattern == null)
                                    continue;

                                var __endpointName = //__route.Name ?? 
                                    __method.UniqueName;

                                // Avoids duplicate endpoint names
                                if (_endpointNames.Contains(__endpointName))
                                    __endpointName = $"{__method.UniqueName}:{order}";

                                var __endpointBuilder = new RouteEndpointBuilder(
                                    _apiMapperService.GetEndpointDelegateProxy(__controller, __method),
                                    //__route.GetRoutePattern(__routePrefix),
                                    __resolvedPattern,
                                    order++)
                                {
                                    DisplayName = __endpointName
                                };
                                __endpointBuilder.Metadata.Add(new EndpointNameMetadata(__endpointName));
                                __endpointBuilder.Metadata.Add(__method);
                                __endpointBuilder.Metadata.Add(__route);

                                // Currently using HttpMethod attribute to specify routes on Actions
                                // Automatically allow pre-flight requests
                                if (__route.HttpMethod != null)
                                {
                                    HttpMethodMetadata httpMethodMetadata = new HttpMethodMetadata(new string[] { __route.HttpMethod.Method }, true);
                                    __endpointBuilder.Metadata.Add(httpMethodMetadata);
                                    this.RequireCors(options =>
                                    {
                                        options.AllowAnyHeader();
                                        options.AllowAnyOrigin();
                                        options.WithMethods(new string[] { __route.HttpMethod.Method });
                                    });
                                }

                                foreach (var convention in _conventions)
                                {
                                    convention(__endpointBuilder);
                                }

                                var __endpoint = __endpointBuilder.Build();
                                endpoints.Add(__endpoint);
                                _endpointNames.Add(__endpointName);
                                //endpoints.Add(__endpointBuilder.Build());
                            }
                        }
                    }
                }
            }

            //foreach (var hubMethod in HubMethods)
            //{
            //    var requiredValues = new { hub = hubMethod.Hub, method = hubMethod.Method };
            //    var order = 1;

            //    foreach (var pattern in Patterns)
            //    {
            //        var resolvedPattern = _routePatternTransformer.SubstituteRequiredValues(pattern, requiredValues);
            //        if (resolvedPattern == null)
            //        {
            //            continue;
            //        }

            //        var endpointBuilder = new RouteEndpointBuilder(
            //            hubMethod.RequestDelegate,
            //            resolvedPattern,
            //            order++);
            //        endpointBuilder.DisplayName = $"{hubMethod.Hub}.{hubMethod.Method}";
            //        endpointBuilder.Metadata.Add()


            //        foreach (var convention in _conventions)
            //        {
            //            convention(endpointBuilder);
            //        }

            //        endpoints.Add(endpointBuilder.Build());
            //    }
            //}

            return endpoints;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IChangeToken GetChangeToken()
        {
            return NullChangeToken.Singleton;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convention"></param>
        public void Add(Action<EndpointBuilder> convention)
        {
            _conventions.Add(convention);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_endpoints == null)
                return "No endpoints";

            var sb = new StringBuilder();
            foreach (var endpoint in _endpoints)
            {
                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    var template = routeEndpoint.RoutePattern.RawText;
                    template = String.IsNullOrEmpty(template) ? "\"\"" : template;
                    sb.Append(template);
                    sb.Append(", Defaults: new { ");
                    sb.AppendJoin(", ", FormatValues(routeEndpoint.RoutePattern.Defaults));
                    sb.Append(" }");
                    var routeNameMetadata = routeEndpoint.Metadata.GetMetadata<IRouteNameMetadata>();
                    sb.Append(", Route Name: ");
                    sb.Append(routeNameMetadata?.RouteName);
                    var routeValues = routeEndpoint.RoutePattern.RequiredValues;
                    if (routeValues.Count > 0)
                    {
                        sb.Append(", Required Values: new { ");
                        sb.AppendJoin(", ", FormatValues(routeValues));
                        sb.Append(" }");
                    }
                    sb.Append(", Order: ");
                    sb.Append(routeEndpoint.Order);

                    var httpMethodMetadata = routeEndpoint.Metadata.GetMetadata<IHttpMethodMetadata>();
                    if (httpMethodMetadata != null)
                    {
                        sb.Append(", Http Methods: ");
                        sb.AppendJoin(", ", httpMethodMetadata.HttpMethods);
                    }
                    sb.Append(", Display Name: ");
                    sb.Append(routeEndpoint.DisplayName);
                    sb.AppendLine();
                }
                else
                {
                    sb.Append("Non-RouteEndpoint. DisplayName:");
                    sb.AppendLine(endpoint.DisplayName);
                }
            }

            return sb.ToString();
        }

        IEnumerable<string> FormatValues(IEnumerable<KeyValuePair<string, object?>> values)
        {
            return values.Select(
                kvp =>
                {
                    var value = "null";
                    if (kvp.Value != null)
                    {
                        value = "\"" + kvp.Value.ToString() + "\"";
                    }
                    return kvp.Key + " = " + value;
                });
        }
    }

    //public class HubMethod
    //{
    //    public string Hub { get; set; }
    //    public string Method { get; set; }
    //    public RequestDelegate RequestDelegate { get; set; }
    //}
}
