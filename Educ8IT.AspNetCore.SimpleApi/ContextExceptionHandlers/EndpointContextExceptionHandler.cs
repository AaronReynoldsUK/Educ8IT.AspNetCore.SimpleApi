// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class EndpointContextExceptionHandler : IEndpointContextExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public EndpointContextExceptionHandler()
        {
            ExceptionsHandled.Add(typeof(Exception));
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Type> ExceptionsHandled { get; private set; } = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public bool IsHandled(Type exceptionType)
        {
            return ExceptionsHandled?.Contains(exceptionType) ?? false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointContext"></param>
        /// <returns></returns>
        public virtual async Task HandleExceptionAsync(EndpointContext endpointContext)
        {
            if (endpointContext == null)
                return;

            if (endpointContext.PipelineException == null)
                return;

            if (endpointContext.HttpContext.Response.HasStarted)
                return;

            await endpointContext.HttpContext.ErrorWriter(endpointContext.PipelineException);
        }
    }
}
