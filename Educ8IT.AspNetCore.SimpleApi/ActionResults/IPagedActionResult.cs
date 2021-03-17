// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPagedActionResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public long? PageNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        public long? PageSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public long? PageCount { get; }

        /// <summary>
        /// 
        /// </summary>
        public long? RecordCount { get; }
    }
}
