// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Supports output formatting to XML (application/xml).
    /// </summary>
    public class OutputFormatterXml : OutputFormatter
    {
        /// <summary>
        /// Set MIME type handled to "application/xml"
        /// </summary>
        public OutputFormatterXml()
            : base("application/xml", true)
        { }

        /// <summary>
        /// Asynchronously converts a CLR object into an XML-based <see cref="ResponseObject"/>
        /// </summary>
        /// <param name="responseObject">The <see cref="ResponseObject"/> to be serialised</param>
        /// <returns>The serialised ResponseObject</returns>
        public override async Task<ResponseObject> FormatResponseAsync(ResponseObject responseObject)
        {
            if (responseObject == null)
                throw new ArgumentNullException(nameof(responseObject));

            if (responseObject.ActionResult == null)
                return responseObject;

            if (responseObject.ActionResult.ResultObject == null)
                return responseObject;

            var __newOutputString = String.Empty;

            __newOutputString = await responseObject.ActionResult.ResultObject.SerialiseToXmlAsync();

            responseObject.FormattedResponseContent = __newOutputString;
            responseObject.ContentLength = __newOutputString?.Length ?? 0;
            responseObject.ContentType ??= this.SupportedMediaType;

            return responseObject;
        }

        /// <summary>
        /// Synchronously converts a CLR object into an XML-based <see cref="ResponseObject"/>
        /// </summary>
        /// <param name="responseObject">The <see cref="ResponseObject"/> to be serialised</param>
        /// <returns>The serialised ResponseObject</returns>
        public override ResponseObject FormatResponse(ResponseObject responseObject)
        {
            if (responseObject == null)
                throw new ArgumentNullException(nameof(responseObject));

            if (responseObject.ActionResult.ResultObject == null)
                return responseObject;

            var __newOutputString = String.Empty;

            __newOutputString = responseObject.ActionResult.SerialiseToXml();

            responseObject.FormattedResponseContent = __newOutputString;
            responseObject.ContentLength = __newOutputString.Length;
            responseObject.ContentType ??= this.SupportedMediaType;

            return responseObject;
        }
    }
}
