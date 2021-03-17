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
    public class ProblemDetailsExceptionHandler : EndpointContextExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public ProblemDetailsExceptionHandler()
        {
            ExceptionsHandled.Add(typeof(ProblemDetailsException));
        }

        /// <summary>
        /// 
        /// </summary>
        public ProblemDetails ProblemDetails { get; private set; }

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
