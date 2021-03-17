// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// Used to return a HATEOS result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HateosActionResult<T> : ActionResult
    {
        /// <summary>
        /// Default Contructor
        /// </summary>
        /// <param name="statusCode">HTTP status code for Response</param>
        /// <param name="resultObject">Object to be encapsulated in HATEOS response</param>
        /// <param name="links">HATEOS links for this response</param>
        public HateosActionResult(
            HttpStatusCode statusCode,
            T resultObject,
            HateosResultObjectLinks links)
            : base(statusCode, new HateosResultObject<T>()
                {
                    Links = links,
                    ResultObject = new List<T>() { resultObject }
                })
        { }
    }
}
