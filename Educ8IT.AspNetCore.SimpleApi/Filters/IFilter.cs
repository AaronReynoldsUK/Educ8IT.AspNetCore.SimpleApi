// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Filters
{
    /// <summary>
    /// These perform a similar function to Filters in ASP.NET Core (Mvc).
    /// They are run before and after an action/method is called.
    /// Only asynchronous filters are supported
    /// e.g. they can be used to handle: 
    /// authorisation, 
    /// response caching, 
    /// soft-request limiting, 
    /// etc
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// PreExecution is called on all registered Filters before executing the action/method.
        /// Return True to bypass (short-circuit) the request pipeline.
        /// </summary>
        /// <param name="endpointContext">An object representing the current Request/Response context</param>
        /// <returns>True to short-curcuit, False to continue pipeline</returns>
        public async Task<bool> PreExecution(EndpointContext endpointContext)
        {
            return await Task.FromResult<bool>(false);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// PostExecution is called on all registered Filters after executing the action/method.
        /// Return True to bypass (short-curcuit) the request pipeline.
        /// </summary>
        /// <param name="endpointContext">An object representing the current Request/Response context</param>
        /// <returns>True to short-curcuit, False to continue pipeline</returns>
        public async Task<bool> PostExecution(EndpointContext endpointContext)
        {
            return await Task.FromResult<bool>(false);
            //throw new NotImplementedException();
        }
    }
}
