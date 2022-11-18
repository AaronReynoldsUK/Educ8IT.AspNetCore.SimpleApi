// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// An encapsulation of the Context through the action/method life-cycle
    /// </summary>
    public interface IEndpointContext
    {
        /// <summary>
        /// Current HTTP Context
        /// </summary>
        public HttpContext HttpContext { get; }

        /// <summary>
        /// Matched Endpoint
        /// </summary>
        public Endpoint Endpoint { get; }

        /// <summary>
        /// Description of the executing action/method
        /// </summary>
        public IApiMethodItem ApiMethodItem { get; }

        /// <summary>
        /// Instance of the <see cref="LinkGenerator"/>
        /// </summary>
        public LinkGenerator LinkGenerator { get; }

        /// <summary>
        /// Result of calling the action/method
        /// </summary>
        public IActionResult ActionResult { get; set; }

        /// <summary>
        /// An exception that occurs during execution of the Request/Response pipeline
        /// e.g. in a Filter or action/method execution.
        /// </summary>
        public Exception PipelineException { get; }

        /// <summary>
        /// Response Content-Type expected/exposed.
        /// </summary>
        public MediaTypeHeaderValue ResponseContentType { get; set; }

        /// <summary>
        /// The parsed Request body
        /// </summary>
        public FormattedBody FormattedBody { get; set; }

        /// <summary>
        /// Skip further filters and actions/methods if TRUE.
        /// </summary>
        public bool ShortCircuit { get; set; }

        /// <summary>
        /// Short-circuit the Pipeline with an Exception.
        /// </summary>
        /// <param name="exception">The Exception to expose to the Pipeline</param>
        public void ShortCircuitWithException(Exception exception);

        /// <summary>
        /// Checks the HTTP Request. If checks fail you can short-circuit with an Exception
        /// </summary>
        public void CheckRequest();

        /// <summary>
        /// Determine actual response type.
        /// Sets <see cref="ResponseContentType"/>.
        /// </summary>
        public void UpdateResponseContentType();

        /// <summary>
        /// Parse the HTML body and store in the EndpointContext
        /// </summary>
        public async Task ParseBodyAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Obtain the method argument values from the Request
        /// </summary>
        /// <returns>Method argument values</returns>
        public async Task<List<object>> GetMethodArgumentsAsync()
        {
            return await Task.FromResult(new List<object>());
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Send the method result through the Output Formatters
        /// </summary>
        /// <returns>A formatted/serialised response to return to the client</returns>
        public async Task<ResponseObject> FormatResponseAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
