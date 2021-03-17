// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatedAtActionResultFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        public async Task<bool> PostExecution(EndpointContext endpointContext)
        {
            if (endpointContext == null)
                throw new ArgumentNullException(nameof(endpointContext));

            if (endpointContext.HttpContext == null)
                throw new InvalidOperationException($"Missing HttpContext for this request");

            if (endpointContext.Endpoint == null)
                throw new InvalidOperationException("There is no Endpoint object for this request");

            // This might not work if type is not exactly ApiMethodItem, experiment with this
            if (endpointContext.ApiMethodItem == null)
                throw new InvalidOperationException("There is no ApiMethodItem object for this request");

            //if (endpointContext.ActionResult is CreatedAtActionResult createdAtActionResult)
            //{
            //    var __apiMethodItem = endpointContext.ApiMethodItem.ApiControllerItem
            //        .GetApiMethodItem(createdAtActionResult.GetMethodName);

            //    string __pathString = ("/" + __apiMethodItem.GetUri(createdAtActionResult.RouteValues))
            //        .Replace("//", "/");

            //    var __request = endpointContext.HttpContext.Request;

            //    var __absoluteUri = UriHelper.BuildAbsolute(
            //        __request.Scheme,
            //        __request.Host,
            //        __request.PathBase,
            //        __pathString);

            //    endpointContext.HttpContext.Response.Headers.Add("Location", __absoluteUri);
            //}
            
            return await Task.FromResult<bool>(false);            
        }
    }
}
