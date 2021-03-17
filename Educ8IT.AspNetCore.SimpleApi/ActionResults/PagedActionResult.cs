// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// Used to return a Paged ActionResult
    /// </summary>
    public class PagedActionResult<T> : ActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="resultObject"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        public PagedActionResult(
            HttpStatusCode statusCode,
            T resultObject,
            long? totalRecordCount, long? pageSize, long? pageNumber)
            : base(statusCode, new PagedResultObject<T>()
            {
                RecordCount = totalRecordCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                ResultObject = resultObject
            })
        { }
    }
}
