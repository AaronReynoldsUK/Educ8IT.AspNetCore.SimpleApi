// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.Filters;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Educ8IT.AspNetCore.SimpleApi.Services;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiMapperOptions : IApiMapperOptions
    {
        /// <inheritdoc/>
        public List<IDocumentationProvider> DocumentationProviders { get; }

        /// <inheritdoc/>
        public List<MediaTypeHeaderValue> SupportedMediaTypes { get; } = new List<MediaTypeHeaderValue>();

        /// <inheritdoc/>
        public List<IFilter> Filters { get; } = new List<IFilter>();

        /// <inheritdoc/>
        public List<IInputFormatter> InputFormatters { get; } = new List<IInputFormatter>();

        /// <inheritdoc/>
        public List<IOutputFormatter> OutputFormatters { get; } = new List<IOutputFormatter>();

        /// <summary>
        /// 
        /// </summary>
        public List<IEndpointContextExceptionHandler> ContextExceptionHandlers { get; } = new List<IEndpointContextExceptionHandler>();

        /// <inheritdoc/>
        public Type ControllerBaseType { get; set; } = null;

        /// <inheritdoc/>
        public bool UseVersioning { get; set; } = true;

        /// <inheritdoc/>
        public ApiVersion DefaultApiVersion { get; set; } = new ApiVersion(1, 0);

        /// <inheritdoc/>
        public bool AssumeDefaultVersionWhenUnspecified { get; set; } = true;

        /// <inheritdoc/>
        public bool ReportApiVersions { get; set; } = true;

        /// <inheritdoc/>
        public string HeaderApiVersionKey { get; set; } = ApiMapperOptionsDefaults.DefaultHeaderApiVersionKey;

        /// <inheritdoc/>
        public string QueryApiVersionKey { get; set; } = ApiMapperOptionsDefaults.DefaultQueryApiVersionKey;

        /// <inheritdoc/>
        public bool AllowVersionOverrides { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public ApiMapperOptions()
        {
            // Setup Default Documentation Provider
            DocumentationProviders = new List<IDocumentationProvider>()
            {
                new XmlDocumentationProvider(
                    Assembly.GetExecutingAssembly(),
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        String.Format("{0}.xml", Assembly.GetExecutingAssembly().GetName().Name))
                    )
            };

            //SupportedMediaTypes = new List<MediaTypeHeaderValue>()
            //{
            //    new MediaTypeHeaderValue("application/xml"),
            //    new MediaTypeHeaderValue("application/json")
            //};

            //InputFormatters.AddRange(new List<InputFormatter>()
            //{
            //    new InputFormatterNewtonsoftJsonPatch(),
            //    new InputFormatterNewtonsoftJson(),
            //    new InputFormatterXml()
            //});

            //OutputFormatters.AddRange(new List<OutputFormatter>()
            //{
            //    new OutputFormatterJson(),
            //    new OutputFormatterXml()
            //});

            // Default supported media types
            SupportedMediaTypes.AddRange(OutputFormatters.Select(f => f.SupportedMediaTypeValue).ToList());
        }
    }
}
