// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.Filters;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiMapperOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public List<IDocumentationProvider> DocumentationProviders { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<MediaTypeHeaderValue> SupportedMediaTypes { get; }

        /// <summary>
        /// A list of handlers that run on the Request Delegate before and after executing the endpoint itself
        /// </summary>
        public List<IFilter> Filters { get; }

        /// <summary>
        /// A list of formatters that convert a request body into usable content
        /// </summary>
        public List<IInputFormatter> InputFormatters { get; }

        /// <summary>
        /// A list of formatters that convert a response object into a HTTP response
        /// </summary>
        public List<IOutputFormatter> OutputFormatters { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<IEndpointContextExceptionHandler> ContextExceptionHandlers { get; }

        /// <summary>
        /// Use this optional property to supply a base type for controllers. 
        /// By default the framework will find all classes ending in "Controller" (ci). 
        /// e.g. you could use the optional base class ApiControllerBase or IApiControllerBase or another class/interface.
        /// </summary>
        public Type ControllerBaseType { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseVersioning { get; }

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion DefaultApiVersion { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReportApiVersions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeaderApiVersionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryApiVersionKey { get; set; }

        /// <summary>
        /// Allows versions specified in Query to override Route to override Header
        /// </summary>
        public bool AllowVersionOverrides { get; set; }
    }
}
