// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ApiMethodItem : TypeDescription, IApiMethodItem
    {
        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public ApiMethodItem() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="apiControllerItem"></param>
        public ApiMethodItem(MethodInfo methodInfo, IApiControllerItem apiControllerItem) : base(methodInfo.GetType(), methodInfo.Name)
        {
            this.ApiControllerItem = apiControllerItem;
            this.MethodInfo = methodInfo;

            ParseAttributes();
        }

        #endregion

        #region Private Fields

        private List<ApiParameterItem> _MethodParameters = null;

        #endregion

        #region Properties

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IApiControllerItem ApiControllerItem { get; private set; }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public MethodInfo MethodInfo { get; private set; }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<KeyValuePair<string, Attribute>> Attributes { get; private set; }

        /// <inheritdoc/>
        public List<ApiParameterItem> MethodParameters
        {
            get
            {
                if (_MethodParameters == null)
                    GetMethodParameters();

                return _MethodParameters;
            }
        }

        ///// <inheritdoc/>
        //[Obsolete]
        //public string ParentTypeName
        //{
        //    get
        //    {
        //        return this.MethodInfo.DeclaringType?.Name ?? "UnknownParentType";
        //    }
        //}

        /// <inheritdoc/>
        public string UniqueName
        {
            get
            {
                return $"{ApiControllerItem.Name}.{Name}";
            }
        }

        /// <inheritdoc/>
        public bool? Ignore { get; private set; }

        /// <inheritdoc/>
        public List<ActionRoute> ActionRoutes { get; private set; }

        /// <inheritdoc/>
        public List<string> AllowedRequestContentTypes { get; private set; }

        /// <inheritdoc/>
        public List<string> AllowedResponseContentTypes { get; private set; }

        /// <inheritdoc/>
        public bool? ExcludeFromDocumentation { get; private set; }

        /// <inheritdoc/>
        public List<ApiVersion> Versions { get; private set; }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Dictionary<int, Type> ResponseTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> ResponseTypesNamed
        {
            get
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                if (ResponseTypes != null)
                {
                    foreach (var item in ResponseTypes)
                    {
                        keyValuePairs.Add(item.Key.ToString(), item.Value.Name);
                    }
                }
                return keyValuePairs;
            }
        }

        /// <inheritdoc/>
        public bool IsAsyncMethod { get; private set; }

        /// <inheritdoc/>
        public Dictionary<MediaTypeHeaderValue, object> SampleRequests { get; private set; }

        /// <inheritdoc/>
        public Dictionary<MediaTypeHeaderValue, object> SampleResponses { get; private set; }

        #endregion

        #region Public Helper Methods

        /// <inheritdoc/>
        public void Document(IApiDescription apiDescription, IDocumentationProvider documentationProvider)
        {
            if (apiDescription == null)
                return;

            if (documentationProvider == null)
                return;

            this.Description =
                documentationProvider.GetSummaryOrDescription(MethodInfo)
                ?? this.Description;

            //apiDescription.GenerateTypeDescription(this.Type, documentationProvider);

            foreach (var apiParameterItem in this.MethodParameters)
                apiParameterItem.Document(apiDescription, documentationProvider);

            foreach (var responseType in this.ResponseTypes)
            {
                if (responseType.Value != null)
                    apiDescription.GenerateTypeDescription(responseType.Value, documentationProvider);
            }
        }

        /// <inheritdoc/>
        public bool IsAllowedHttpMethod(string httpMethod)
        {
            return ActionRoutes?.Exists(r => r.HttpMethod.Method == httpMethod) ?? false;
        }

        /// <inheritdoc/>
        public async Task<List<object>> GetMethodArgumentsAsync(EndpointContext endpointContext)
        {
            HttpContext httpContext = endpointContext.HttpContext;
            FormattedBody formattedBody = endpointContext.FormattedBody;

            if (this.MethodParameters == null)
                return new List<object>();

            var __methodArguments = new List<object>(MethodParameters.Count);
            for (int i = 0; i < MethodParameters.Count; i++)
                __methodArguments.Add(null);

            var __mapperIOptions = httpContext.RequestServices.GetRequiredService<IOptions<ApiMapperOptions>>();
            var __mapperOptions = __mapperIOptions.Value;

            UpdateMethodArgumentsFromContext(__methodArguments, httpContext);
            UpdateMethodArgumentsFromHeader(__methodArguments, httpContext);
            UpdateMethodArgumentsFromRoute(__methodArguments, httpContext);
            await UpdateMethodArgumentsFromBody(__methodArguments, httpContext, formattedBody, __mapperOptions);
            UpdateMethodArgumentsFromForm(__methodArguments, httpContext, formattedBody);
            UpdateMethodArgumentsFromQueryString(__methodArguments, httpContext);

            // Check for required...
            for (int i = 0; i < MethodParameters.Count; i++)
                if (MethodParameters[i].IsRequired && __methodArguments[i] == null)
                    throw new CustomHttpException($"Argument {MethodParameters[i].Alias ?? MethodParameters[i].Name} is required", HttpStatusCode.BadRequest);

            // TODO: Validate Arguments and return a Problem Details if wrong

            // Determine if Arguments match - need to check this works accurately
            if (MethodParameters.Count != __methodArguments.Count)
                throw new CustomHttpException("Invalid number of Arguments supplied", HttpStatusCode.BadRequest);

            return __methodArguments;
        }

        /// <inheritdoc/>
        public bool IsRequestContentTypeMatch(string requestContentType)
        {
            if (AllowedRequestContentTypes == null || AllowedRequestContentTypes.Count == 0)
                return true;

            return AllowedRequestContentTypes.Contains(requestContentType);
        }

        /// <inheritdoc/>
        public bool IsResponseContentTypeMatch(IList<MediaTypeHeaderValue> acceptHeaders)
        {
            if (AllowedResponseContentTypes == null || AllowedResponseContentTypes.Count == 0)
                return true;

            if (acceptHeaders == null)
                return false;

            //MediaTypeHeaderValue __universalMTHV = MediaTypeHeaderValue.Parse("*/*");
            bool __universalMatch = false;
            foreach (var __acceptHeader in acceptHeaders)
            {
                if (__acceptHeader.MediaType.ToString() == "*/*")
                    __universalMatch = true;

                if (AllowedResponseContentTypes.Contains(__acceptHeader.MediaType.ToString() ?? String.Empty))
                    return true;
            }

            return __universalMatch;
        }

        /// <inheritdoc/>
        public MediaTypeHeaderValue GetPrimaryResponseContentType(IList<MediaTypeHeaderValue> acceptHeaders)
        {
            if (acceptHeaders == null)
            {
                // Accept whatever we send
                acceptHeaders = new List<MediaTypeHeaderValue>(
                    MediaTypeHeaderValue.ParseList(new List<string>
                    {
                        //"application/json",
                        //"application/xml",
                        "*/"
                    }));
            }

            if (AllowedResponseContentTypes == null || AllowedResponseContentTypes.Count == 0)
            {
                AllowedResponseContentTypes.AddRange(new string[] 
                { 
                    //"application/json", 
                    //"application/xml", 
                    "*/*" 
                });
            }

            bool __universalMatch = false;
            foreach (var __acceptHeader in acceptHeaders)
            {
                if (__acceptHeader.MediaType.ToString() == "*/*")
                    __universalMatch = true;

                if (AllowedResponseContentTypes.Contains(__acceptHeader.MediaType.ToString() ?? String.Empty))
                    return __acceptHeader;
            }

            return __universalMatch ? MediaTypeHeaderValue.Parse(AllowedResponseContentTypes.First()) : null
                ?? MediaTypeHeaderValue.Parse("*/*");
        }

        /// <inheritdoc/>
        public void GenerateSamples()
        {
            if (SampleRequests == null)
            {
                SampleRequests ??= new Dictionary<MediaTypeHeaderValue, object>();

                // generate...
            }
            if (SampleResponses == null)
            {
                SampleResponses = new Dictionary<MediaTypeHeaderValue, object>();

                // generate...
            }

            // I don't want these stored in IApiMethodItem though.
            // They are only needed for Documentation requests
            // They should be stored in ApiDocumentation/ApiDescription and generated+cached on request

            //HelpPageSampleGenerator sampleGenerator = new HelpPageSampleGenerator();

            //try
            //{
            //    foreach (var item in sampleGenerator.GetSampleRequests(this))
            //    {
            //        this.SampleRequests.Add(item.Key, item.Value);
            //    }

            //    foreach (var item in sampleGenerator.GetSampleResponses(this))
            //    {
            //        this.SampleResponses.Add(item.Key, item.Value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //    //apiModel.ErrorMessages.Add(String.Format(CultureInfo.CurrentCulture,
            //    //    "An exception has occurred while generating the sample. Exception message: {0}",
            //    //    HelpPageSampleGenerator.UnwrapException(e).Message));
            //}
        }

        #endregion

        #region Private Helper Methods

        private void ParseAttributes()
        {
            Attributes = new List<KeyValuePair<string, Attribute>>();

            MethodInfo.GetCustomAttributes().ToList()
                    .ForEach(i => Attributes.Add(new KeyValuePair<string, Attribute>(i.GetType().Name, i)));

            ActionRoutes = new List<ActionRoute>();
            AllowedRequestContentTypes = new List<string>();
            AllowedResponseContentTypes = new List<string>();
            ExcludeFromDocumentation = default;
            Ignore = default;
            IsAsyncMethod = false;
            ResponseTypes = new Dictionary<int, Type>();
            Versions = new List<ApiVersion>();

            if (ApiControllerItem.AllowedRequestContentTypes != null)
                ApiControllerItem.AllowedRequestContentTypes.ForEach((contentType) =>
                {
                    if (!AllowedRequestContentTypes.Contains(contentType))
                        AllowedRequestContentTypes.Add(contentType);
                });

            if (ApiControllerItem.AllowedResponseContentTypes != null)
                ApiControllerItem.AllowedResponseContentTypes.ForEach((contentType) =>
                {
                    if (!AllowedResponseContentTypes.Contains(contentType))
                        AllowedResponseContentTypes.Add(contentType);
                });

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "IgnoreAttribute")
                {
                    Ignore = (__attribute.Value as IgnoreAttribute)?.ShouldIgnore;
                }
                else if (__attribute.Key == "AllowedRequestContentTypeAttribute")
                {
                    AllowedRequestContentTypes.Add((__attribute.Value as AllowedRequestContentTypeAttribute).ContentType);
                }
                else if (__attribute.Value is AllowedRequestContentTypeAttribute allowedRequestContentTypeAttribute)
                {
                    if (!AllowedRequestContentTypes.Contains(allowedRequestContentTypeAttribute.ContentType))
                        AllowedRequestContentTypes.Add(allowedRequestContentTypeAttribute.ContentType);
                }
                else if (__attribute.Value is AllowedResponseContentTypeAttribute allowedResponseContentTypeAttribute)
                {
                    if (!AllowedResponseContentTypes.Contains(allowedResponseContentTypeAttribute.ContentType))
                        AllowedResponseContentTypes.Add(allowedResponseContentTypeAttribute.ContentType);
                }
                //else if (__attribute.Key == "AllowedResponseContentTypeAttribute")
                //{
                //    AllowedResponseContentTypes.Add((__attribute.Value as AllowedResponseContentTypeAttribute).ContentType);
                //}
                //else if (__attribute.Key == "ExcludeFromDocumentationAttribute")
                //{
                //    ExcludeFromDocumentation = (__attribute.Value as ExcludeFromDocumentationAttribute)?.ShouldExclude;
                //}
                else if (__attribute.Key == "VersionAttribute")
                {
                    var __apiVersion = (__attribute.Value as VersionAttribute)?.Version;
                    if (__apiVersion != null)
                        Versions.Add(__apiVersion);
                }
                else if (httpMethodAttributes.Contains(__attribute.Key))
                {
                    var __methodAttribute = __attribute.Value as HttpMethodAttribute;
                    foreach (var __method in __methodAttribute.HttpMethods)
                    {
                        if (__methodAttribute.RouteTemplates != null)
                        {
                            foreach (var __route in __methodAttribute.RouteTemplates)
                            {
                                ActionRoutes.Add(new ActionRoute()
                                {
                                    HttpMethod = GetMethod(__method),
                                    Name = __methodAttribute.Name ?? this.Name,
                                    Order = __methodAttribute.Order,
                                    ParentRoutePrefixes = ApiControllerItem.RoutePrefixes,
                                    Pattern = __route
                                });
                            }
                        }
                        else
                        {
                            ActionRoutes.Add(new ActionRoute()
                            {
                                HttpMethod = GetMethod(__method),
                                Name = __methodAttribute.Name ?? this.Name,
                                Order = __methodAttribute.Order,
                                ParentRoutePrefixes = ApiControllerItem.RoutePrefixes,
                                Pattern = String.Empty
                            });
                        }
                    }
                }
                else if (__attribute.Key == "ResponseTypeAttribute")
                {
                    var __rta = __attribute.Value as ResponseTypeAttribute;
                    if (ResponseTypes.ContainsKey(__rta.HttpResponseCode))
                        throw new Exception(
                            "Only one ResponseType is allowed per HttpStatusCode on an Action. " +
                            $"Controller: {this.MethodInfo.DeclaringType.Name}; Action: {this.MethodInfo.Name}");

                    ResponseTypes.Add(__rta.HttpResponseCode, __rta.ResponseType);
                }
                else if (__attribute.Key == "AsyncStateMachineAttribute")
                {
                    IsAsyncMethod = true;
                }
            }

            if (ActionRoutes.Count() == 0)
            {
                System.Net.Http.HttpMethod __httpMethod = System.Net.Http.HttpMethod.Get;
                if (this.Name.StartsWith(HttpMethods.Delete, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Delete;
                }
                //else if (this.Name.StartsWith(HttpMethods.Get, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    __httpMethod = HttpMethod.Get;
                //}
                else if (this.Name.StartsWith(HttpMethods.Head, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Head;
                }
                else if (this.Name.StartsWith(HttpMethods.Options, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Options;
                }
                else if (this.Name.StartsWith(HttpMethods.Patch, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Patch;
                }
                else if (this.Name.StartsWith(HttpMethods.Post, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Post;
                }
                else if (this.Name.StartsWith(HttpMethods.Put, StringComparison.InvariantCultureIgnoreCase))
                {
                    __httpMethod = System.Net.Http.HttpMethod.Put;
                }

                ActionRoutes.Add(new ActionRoute()
                {
                    HttpMethod = __httpMethod,
                    Name = this.Name,
                    Order = 0,
                    Pattern = String.Empty
                });
            }
        }

        private void GetMethodParameters()
        {
            this._MethodParameters = new List<ApiParameterItem>();

            var __parameterInfo = MethodInfo.GetParameters();

            foreach (var __paramInfo in __parameterInfo)
                this.MethodParameters.Add(new ApiParameterItem(__paramInfo));
        }

        private System.Net.Http.HttpMethod GetMethod(string httpMethod)
        {
            System.Net.Http.HttpMethod method = null;

            switch (httpMethod.ToUpper())
            {
                case "DELETE":
                    method = System.Net.Http.HttpMethod.Delete;
                    break;
                case "GET":
                    method = System.Net.Http.HttpMethod.Get;
                    break;
                case "HEAD":
                    method = System.Net.Http.HttpMethod.Head;
                    break;
                case "OPTIONS":
                    method = System.Net.Http.HttpMethod.Options;
                    break;
                case "PATCH":
                    method = System.Net.Http.HttpMethod.Patch;
                    break;
                case "POST":
                    method = System.Net.Http.HttpMethod.Post;
                    break;
                case "PUT":
                    method = System.Net.Http.HttpMethod.Put;
                    break;
            }

            return method;
        }

        private void  UpdateMethodArgumentsFromContext(List<object> methodArguments, HttpContext context)
        {
            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsContextParameter)
                    continue;


                var __simpleParameterValue = context.Items.ContainsKey(__param.Alias ?? __param.Name)
                    ? context.Items[__param.Alias ?? __param.Name]
                    : null;

                
                if (__simpleParameterValue != null)
                {
                    TypeConverter typeConverter = new TypeConverter();
                    methodArguments[__paramIndex] = typeConverter.ConvertTo(__simpleParameterValue, __param.Type);                    
                }   
            }
        }

        private void UpdateMethodArgumentsFromHeader(List<object> methodArguments, HttpContext context)
        {
            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsHeaderParameter)
                    continue;

                var __simpleParameterValues = context.Request.Headers[__param.Alias ?? __param.Name];
                var __simpleParameterValue = __simpleParameterValues.Count > 0 ? String.Join("", __simpleParameterValues) : null;

                if (!String.IsNullOrEmpty(__simpleParameterValue))
                    methodArguments[__paramIndex] = Conversion.ConvertToType(__simpleParameterValue, __param.Type);
            }
        }

        private void UpdateMethodArgumentsFromRoute(List<object> methodArguments, HttpContext context)
        {
            var __routeData = context.GetRouteData();

            if (__routeData == null)
                return;

            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsRouteParameter)
                    continue;

                var __simpleParameterValue = __routeData.Values[__param.Alias ?? __param.Name] as string;

                if (!String.IsNullOrEmpty(__simpleParameterValue))
                    methodArguments[__paramIndex] = Conversion.ConvertToType(__simpleParameterValue, __param.Type);
            }
        }

        private async Task UpdateMethodArgumentsFromBody(List<object> methodArguments, HttpContext httpContext, FormattedBody formattedBody, ApiMapperOptions apiMapperOptions)
        {
            if (formattedBody == null)
                return;

            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsBodyParameter)
                    continue;

                if (this.MethodParameters.Count(p => p.IsBodyParameter) > 1)
                    throw new Exception("Only one parameter per method/action can be a Body parameter. Instead of specifying multiple Body parameters, use a DTO.");

                bool __formatMatched = false;
                if (apiMapperOptions.InputFormatters != null)
                {
                    foreach (IInputFormatter inputFormatter in apiMapperOptions.InputFormatters)
                    {
                        if (inputFormatter.SupportedMediaTypeValue.MediaType != httpContext.Request.ContentType)
                            continue;

                        __formatMatched = true;

                        if (inputFormatter.HandlesAsyncFormatting)
                            methodArguments[__paramIndex] = await inputFormatter.FormatRequestAsync(formattedBody.FormBody, MethodParameters[__paramIndex].Type);
                        else
                            methodArguments[__paramIndex] = inputFormatter.FormatRequest(formattedBody.FormBody, MethodParameters[__paramIndex].Type);
                    }
                    if (!__formatMatched && httpContext.Request.ContentType != null)
                    {
                        foreach (IInputFormatter inputFormatter in apiMapperOptions.InputFormatters)
                        {
                            if (!inputFormatter.SupportedMediaTypeValue.MatchesTypeAndSuffixOrSubType(httpContext.Request.ContentType))
                                continue;

                            __formatMatched = true;

                            if (inputFormatter.HandlesAsyncFormatting)
                                methodArguments[__paramIndex] = await inputFormatter.FormatRequestAsync(formattedBody.FormBody, MethodParameters[__paramIndex].Type);
                            else
                                methodArguments[__paramIndex] = inputFormatter.FormatRequest(formattedBody.FormBody, MethodParameters[__paramIndex].Type);
                        }
                    }
                }
                if (!__formatMatched)
                {
                    if (!String.IsNullOrEmpty(formattedBody.XmlFromBody))
                    {
                        methodArguments[__paramIndex] = formattedBody.XmlFromBody.DeserialiseFromXml(__param.Type);
                    }
                    else if (!String.IsNullOrEmpty(formattedBody.JsonFromBody))
                    {
                        methodArguments[__paramIndex] = formattedBody.JsonFromBody.DeserialiseFromJson(__param.Type);
                    }
                    else
                    {
                        //methodArguments[__paramIndex] = formattedBody.Form.ConvertToType(__param);
                        methodArguments[__paramIndex] = __param.GetInstanceFromNVC(formattedBody.Form);
                    }
                }
            }


            //var __firstBodyParameter = this.MethodParameters.FirstOrDefault(p => p.IsBodyParameter);
            //if (__firstBodyParameter != null)
            //{
            //    if (!String.IsNullOrEmpty(formattedBody.XmlFromBody))
            //    {
            //        __methodArguments.Add(formattedBody.XmlFromBody.DeserialiseFromXml(__firstBodyParameter.Type));
            //    }
            //    else if (!String.IsNullOrEmpty(formattedBody.JsonFromBody))
            //    {
            //        __methodArguments.Add(formattedBody.JsonFromBody.DeserialiseFromJson(__firstBodyParameter.Type));
            //    }
            //    else
            //    {
            //        __methodArguments.Add(formattedBody.Form.ConvertToType(__firstBodyParameter));
            //    }
            //}

            //return __methodArguments;
        }

        private void UpdateMethodArgumentsFromForm(List<object> methodArguments, HttpContext httpContext, FormattedBody formattedBody)
        {
            if (formattedBody == null)
                return;

            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsFormParameter)
                    continue;

                var __simpleParameterValue = formattedBody.Form[__param.Alias ?? __param.Name];
                if (!String.IsNullOrEmpty(__simpleParameterValue))
                {
                    methodArguments[__paramIndex] = Conversion.ConvertToType(__simpleParameterValue, __param.Type);
                }
            }
            //foreach (var __param in this.MethodParameters.Where(p => p.IsFormParameter))
            //{
            //    var __simpleParameterValue = formattedBody.Form[__param.Alias ?? __param.Name];
            //    if (String.IsNullOrEmpty(__simpleParameterValue))
            //    {
            //        __methodArguments.Add(null);
            //    }
            //    else
            //    {
            //        __methodArguments.Add(DataMapping.ConvertParameter(__simpleParameterValue, __param.Type));
            //    }
            //}

            //return __methodArguments;
        }

        private void UpdateMethodArgumentsFromQueryString(List<object> methodArguments, HttpContext context)
        {
            int __paramIndex = -1;
            foreach (var __param in this.MethodParameters)
            {
                ++__paramIndex;

                if (!__param.IsQueryParameter)
                    continue;

                var __simpleParameterValue = context.Request.Query[__param.Alias ?? __param.Name];

                if (!String.IsNullOrEmpty(__simpleParameterValue))
                    methodArguments[__paramIndex] = Conversion.ConvertToType(__simpleParameterValue, __param.Type);

                if (__param.Type.IsDictionary())
                {
                    IDictionary<string, string> t = new Dictionary<string, string>();
                    foreach (string k in context.Request.Query.Keys)
                    {
                        t[k] = context.Request.Query[k].ToString();
                    }
                    methodArguments[__paramIndex] = t;
                }
            }

            //if (this.MethodParameters.Count == 1 && this.MethodParameters.First().IsQueryParameter && this.MethodParameters.First().Type.IsDictionary())
            //{
            //    // Convert to Dictionary
            //    //context.Request.Query.Keys.ToDictionary(k => k, k => context.Request.Query[k]);

            //    IDictionary<string, string> t = new Dictionary<string, string>();
            //    foreach (string k in context.Request.Query.Keys)
            //    {
            //        t[k] = context.Request.Query[k].ToString();
            //    }
            //    if (t != null)
            //    {
            //        methodArguments[0] = t;
            //    }
            //}
            //else
            //{

            //}




            //var __methodArguments = new List<object>();

            //foreach (var __param in this.MethodParameters.Where(p => p.IsQueryParameter))
            //{
            //    var __simpleParameterValue = context.Request.Query[__param.Alias ?? __param.Name];
            //    if (String.IsNullOrEmpty(__simpleParameterValue))
            //    {
            //        __methodArguments.Add(null);
            //    }
            //    else
            //    {
            //        __methodArguments.Add(DataMapping.ConvertParameter(__simpleParameterValue, __param.Type));
            //    }
            //}

            //return __methodArguments;
        }

        /// <inheritdoc/>
        public string GetUri(RouteValueDictionary rvdParameters = null)
        {
            var __orderedActionRoutes = this.ActionRoutes
                .OrderBy(o => o.Method)
                .ThenBy(o => o.Order)
                .ThenBy(o => o.ToString());

            var __actionRoute = __orderedActionRoutes.FirstOrDefault();

            if (__actionRoute == null)
                throw new ProblemDetailsException(new Models.ProblemDetails()
                {
                    Title = "No matching Action Routes",
                    Detail = $"No action routes found for {this.UniqueName}",
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var __parentRoutePrefix = (__actionRoute.ParentRoutePrefixes != null && __actionRoute.ParentRoutePrefixes.Count > 0)
                ? __actionRoute.ParentRoutePrefixes.FirstOrDefault()
                : null;

            var __defaults = new RouteValueDictionary()
                {
                    { "controller", this.ApiControllerItem.Name },
                    { "action", this.Name }
                };
            //LinkGeneratorEndpointNameAddressExtensions.
            //LinkGenerator linkGenerator = new LinkGenerator()

            var __routePattern = __actionRoute.GetRoutePattern(
                __parentRoutePrefix, __defaults, null, rvdParameters);

            var __ = TemplateParser.Parse(__routePattern.RawText);
            
            TemplateMatcher templateMatcher = new TemplateMatcher(__, __defaults);

            var __pattern = __routePattern.RawText
                .Replace("{controller}", this.ApiControllerItem.Name)
                .Replace("{action}", this.Name);

            return __pattern;
        }

        #endregion

        private static readonly string[] httpMethodAttributes = new string[]
        {
            "HttpDeleteAttribute",
            "HttpGetAttribute",
            "HttpHeadAttribute",
            "HttpOptionsAttribute",
            "HttpPatchAttribute",
            "HttpPostAttribute",
            "HttpPutAttribute",
        };
    }
}
