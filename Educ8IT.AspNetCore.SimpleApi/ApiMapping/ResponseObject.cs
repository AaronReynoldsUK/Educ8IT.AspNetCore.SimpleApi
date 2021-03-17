// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.ApiMapping
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseObject
    {
        private readonly HttpContext _context;
        private readonly IActionResult _actionResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionResult"></param>
        public ResponseObject(HttpContext context, IActionResult actionResult)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _actionResult = actionResult ?? throw new ArgumentNullException(nameof(actionResult));
        }

        /// <summary>
        /// 
        /// </summary>
        public object FormattedResponseContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ResponseContentClrType
        {
            get
            {
                return FormattedResponseContent?.GetType() ?? null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IHeaderDictionary Headers { get; } = new HeaderDictionary();

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// 
        /// </summary>
        public HttpContext HttpContext { get { return _context; } }

        /// <summary>
        /// 
        /// </summary>
        public IActionResult ActionResult { get { return _actionResult; } }
    }
}
