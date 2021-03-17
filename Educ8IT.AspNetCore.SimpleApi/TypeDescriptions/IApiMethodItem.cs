// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiMethodItem : ITypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <param name="documentationProvider"></param>
        public void Document(IApiDescription apiDescription, IDocumentationProvider documentationProvider);

        /// <summary>
        /// A ref to the parent Controller
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IApiControllerItem ApiControllerItem { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<KeyValuePair<string, Attribute>> Attributes { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ApiParameterItem> MethodParameters { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string ParentTypeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string UniqueName { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool? Ignore { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ActionRoute> ActionRoutes { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedRequestContentTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedResponseContentTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ExcludeFromDocumentation { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ApiVersion> Versions { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Dictionary<int, Type> ResponseTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> ResponseTypesNamed { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAsyncMethod { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContentType"></param>
        /// <returns></returns>
        public bool IsRequestContentTypeMatch(string requestContentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptHeaders"></param>
        /// <returns></returns>
        public bool IsResponseContentTypeMatch(IList<MediaTypeHeaderValue> acceptHeaders);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptHeaders"></param>
        /// <returns></returns>
        public MediaTypeHeaderValue GetPrimaryResponseContentType(IList<MediaTypeHeaderValue> acceptHeaders);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public bool IsAllowedHttpMethod(string httpMethod);

        /// <summary>
        /// Used by the Delegate to bind passed arguments from the Header, Body, Form, QueryString and Route
        /// </summary>
        /// <param name="endpointContext">Extended context for this Request</param>
        /// <returns>Method arguments parsed from the Request</returns>
        public async Task<List<object>> GetMethodArgumentsAsync(EndpointContext endpointContext)
        {
            return await Task.FromResult(new List<object>());
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<MediaTypeHeaderValue, object> SampleRequests { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<MediaTypeHeaderValue, object> SampleResponses { get; }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateSamples();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUri(RouteValueDictionary rvdParameters = null);
    }
}
