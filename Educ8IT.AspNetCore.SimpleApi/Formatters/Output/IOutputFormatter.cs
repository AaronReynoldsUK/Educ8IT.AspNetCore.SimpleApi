// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Output Formatters convert the Response (body) into serialised data for transmission over HTTP.
    /// </summary>
    public interface IOutputFormatter
    {
        /// <summary>
        /// String representation of the MIME type supported
        /// e.g. application/json
        /// </summary>
        public string SupportedMediaType { get; }

        /// <summary>
        /// The <see cref="MediaTypeHeaderValue"/> this output formtter supports.
        /// </summary>
        public MediaTypeHeaderValue SupportedMediaTypeValue { get; }

        /// <summary>
        /// Whether this output formatter supports async calls.
        /// </summary>
        public bool HandlesAsyncFormatting { get; }

        /// <summary>
        /// Converts the Response asynchronously.
        /// </summary>
        /// <param name="responseObject">The <see cref="ResponseObject"/> to be serialised</param>
        /// <returns>The serialised ResponseObject</returns>
        public async Task<ResponseObject> FormatResponseAsync(ResponseObject responseObject)
        {
            return await Task.FromResult<ResponseObject>(responseObject);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the Response synchronously.
        /// </summary>
        /// <param name="responseObject">The <see cref="ResponseObject"/> to be serialised</param>
        /// <returns>The serialised ResponseObject</returns>
        public ResponseObject FormatResponse(ResponseObject responseObject)
        {
            throw new NotImplementedException();
        }
    }
}
