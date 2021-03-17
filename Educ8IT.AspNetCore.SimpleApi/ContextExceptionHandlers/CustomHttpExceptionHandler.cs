// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomHttpExceptionHandler : EndpointContextExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomHttpExceptionHandler()
        {
            ExceptionsHandled.Add(typeof(CustomHttpException));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        public override async Task HandleExceptionAsync(EndpointContext endpointContext)
        {
            await base.HandleExceptionAsync(endpointContext);
        }
    }
}
