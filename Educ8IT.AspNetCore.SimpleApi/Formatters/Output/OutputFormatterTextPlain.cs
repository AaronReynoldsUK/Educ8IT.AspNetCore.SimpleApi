// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class OutputFormatterTextPlain : OutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public OutputFormatterTextPlain()
            : base("text/plain", false)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportedMimeType"></param>
        /// <param name="handlesAsyncFormatting"></param>
        public OutputFormatterTextPlain(string supportedMimeType, bool handlesAsyncFormatting = false)
            : base(supportedMimeType, handlesAsyncFormatting)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseObject"></param>
        /// <returns></returns>
        public override ResponseObject FormatResponse(ResponseObject responseObject)
        {
            if (responseObject == null)
                throw new ArgumentNullException(nameof(responseObject));

            if (responseObject.ActionResult.ResultObject == null)
                return responseObject;

            var __content = responseObject?.ToString() ?? String.Empty;

            responseObject.FormattedResponseContent = __content;
            responseObject.ContentLength = __content.Length;
            responseObject.ContentType ??= "text/plain";

            return responseObject;
        }
    }
}
