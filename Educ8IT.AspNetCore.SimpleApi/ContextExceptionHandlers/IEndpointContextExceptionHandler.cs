// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEndpointContextExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Type> ExceptionsHandled { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        public bool IsHandled(Type exceptionType)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public async Task HandleExceptionAsync(Exception ex)
        {
            await Task.CompletedTask;
        }
    }
}
