// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// Interface for all Action Results
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// HTTP Status Code returned to Client
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Actual response object to be converted/formatted and returned to Client
        /// </summary>
        public object ResultObject { get; }

        /// <summary>
        /// CLR type of <see cref="ResultObject"/>
        /// </summary>
        public Type ResultType { get; }
    }
}
