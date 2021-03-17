// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Output Formatter for "application/json".
    /// Uses the <see cref="Newtonsoft.Json.JsonSerializer"/>
    /// </summary>
    public class OutputFormatterNewtonsoftJson : OutputFormatter
    {
        /// <summary>
        /// Set MIME type handled to "application/json"
        /// </summary>
        public OutputFormatterNewtonsoftJson()
            : base("application/json", false)
        { }

        /// <summary>
        /// Set MIME type handled to "supportedMimeType"
        /// </summary>
        /// <param name="supportedMimeType">Allows sub-setting of the JSON mime-type</param>
        /// <param name="handlesAsyncFormatting">Whether this formatter supports asynchronous formatting</param>
        public OutputFormatterNewtonsoftJson(string supportedMimeType, bool handlesAsyncFormatting = false)
            : base(supportedMimeType, handlesAsyncFormatting)
        { }

        /// <summary>
        /// Synchronously converts a CLR object into a JSON-based <see cref="ResponseObject"/>
        /// </summary>
        /// <param name="responseObject">The <see cref="ResponseObject"/> to be serialised</param>
        /// <returns>The serialised ResponseObject</returns>
        public override ResponseObject FormatResponse(ResponseObject responseObject)
        {
            if (responseObject == null)
                throw new ArgumentNullException(nameof(responseObject));

            if (responseObject.ActionResult.ResultObject == null)
                return responseObject;
            
            string __newOutputString = JsonConvert.SerializeObject(responseObject.ActionResult.ResultObject, responseObject.ActionResult.ResultType, new JsonSerializerSettings());

            responseObject.FormattedResponseContent = __newOutputString;
            responseObject.ContentLength = __newOutputString.Length;
            responseObject.ContentType ??= this.SupportedMediaType;

            return responseObject;
        }
    }
}
