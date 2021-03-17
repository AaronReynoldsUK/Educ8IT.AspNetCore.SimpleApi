// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Services;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// An encapsulation of the Context through the action/method life-cycle
    /// </summary>
    public class EndpointContext: IEndpointContext
    {
        /// <summary>
        /// Initialises this context
        /// </summary>
        /// <param name="httpContext"></param>
        public EndpointContext(HttpContext httpContext)
        {
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

            LinkGenerator = httpContext.RequestServices.GetService<LinkGenerator>();

            Endpoint = httpContext.GetEndpoint();

            if (Endpoint == null)
                return;

            ApiMethodItem = Endpoint.Metadata.GetMetadata<ApiMethodItem>();
        }

        /// <inheritdoc/>
        public HttpContext HttpContext { get; private set; }

        /// <inheritdoc/>
        public Endpoint Endpoint { get; private set; }

        /// <inheritdoc/>
        public IApiMethodItem ApiMethodItem { get; private set; }

        /// <inheritdoc/>
        public LinkGenerator LinkGenerator { get; private set; }

        /// <inheritdoc/>
        public IActionResult ActionResult { get; set; }

        private Exception _PipelineException = null;
        /// <summary>
        /// An exception that occurs during execution of the Request/Response pipeline
        /// e.g. in a Filter or action/method execution.
        /// </summary>
        public Exception PipelineException
        {
            get
            {
                return _PipelineException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MediaTypeHeaderValue ResponseContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FormattedBody FormattedBody { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShortCircuit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void ShortCircuitWithException(Exception exception)
        {
            _PipelineException = exception;
            ShortCircuit = true;
        }

        /// <summary>
        /// Checks:
        /// - correct HTTP method used
        /// - request content-type matches endpoint
        /// - response content-type is allowed by client
        /// </summary>
        public void CheckRequest()
        {
            if (ShortCircuit)
                return;

            // Determine if HTTPMethod matches
            if (!ApiMethodItem.IsAllowedHttpMethod(HttpContext.Request.Method))
                ShortCircuitWithException(new CustomHttpException("HTTP Method not allowed", HttpStatusCode.MethodNotAllowed));

            // Determine if Request Content Types matches
            else if (!ApiMethodItem.IsRequestContentTypeMatch(HttpContext.Request.ContentType))
                ShortCircuitWithException(new CustomHttpException("Request ContentType not allowed", HttpStatusCode.UnprocessableEntity));

            // Determine if Response Content Type matches
            else if (!ApiMethodItem.IsResponseContentTypeMatch(HttpContext.Request.GetTypedHeaders().Accept))
                ShortCircuitWithException(new CustomHttpException("Response ContentType not allowed (Check Accept header)", HttpStatusCode.UnprocessableEntity));
        }

        /// <summary>
        /// Determine actual response type.
        /// Can actually remove previous call to IsResponseContentTypeMatch as this will return NULL if no options.
        /// Sets:
        /// - EndpointContext.ResponseContentType
        /// </summary>
        public void UpdateResponseContentType()
        {
            if (ShortCircuit)
                return;

            ResponseContentType = ApiMethodItem
                .GetPrimaryResponseContentType(HttpContext.Request.GetTypedHeaders().Accept);

            if (ResponseContentType == null)
                ShortCircuitWithException(
                    new CustomHttpException(
                        "No ContentType specified for Action. This is an API fault", 
                        HttpStatusCode.InternalServerError));
        }

        /// <inheritdoc/>
        public async Task ParseBodyAsync()
        {
            if (ShortCircuit)
                return;

            FormattedBody __formattedBody = new FormattedBody(HttpContext);
            await __formattedBody.Parse();

            FormattedBody = __formattedBody;
        }

        /// <summary>
        /// Uses the helper method on <see cref="IApiMethodItem"/> to get the method argument values.
        /// </summary>
        /// <returns>Method arguments parsed from the Request</returns>
        public async Task<List<object>> GetMethodArgumentsAsync()
        {
            if (ShortCircuit)
                return null;

            // Convert/Pass parameters (so we don't waste time on this unless it matches all previous)
            var __methodArguments = await ApiMethodItem.GetMethodArgumentsAsync(this);

            // Determine if Arguments match
            // TODO: need to check this works accurately
            if (ApiMethodItem.MethodParameters.Count != __methodArguments.Count)
                ShortCircuitWithException(
                    new CustomHttpException(
                        "Invalid number of Arguments supplied", 
                        HttpStatusCode.BadRequest));

            return __methodArguments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseObject> FormatResponseAsync()
        {
            return await HttpContext.FormatResponseAsync(this.ActionResult, this.ResponseContentType);
        }

        /// <summary>
        /// Used for Hateos
        /// </summary>
        /// <param name="methodName">Name of the Method within current controller</param>
        /// <param name="values">Optional route values</param>
        /// <param name="queryParameters">Optional Query parameters</param>
        /// <returns></returns>
        public string GetUri(string methodName, object values = null, object queryParameters = null)
        {
            if (LinkGenerator == null)
                return null;

            if (String.IsNullOrEmpty(methodName))
                return null;

            var __methodItem = ApiMethodItem.ApiControllerItem.GetApiMethodItem(methodName);

            if (__methodItem == null)
                return null;

            var __uri = LinkGenerator.GetUriByName(
                this.HttpContext,
                __methodItem.UniqueName,
                values);

            if (__uri == null)
                return null;

            if (queryParameters == null)
                return __uri;

            var __dictionary = new EasyDictionary(queryParameters);
            if (__dictionary.Count == 0)
                return __uri;

            return QueryHelpers.AddQueryString(__uri, __dictionary);
        }
    }
}
