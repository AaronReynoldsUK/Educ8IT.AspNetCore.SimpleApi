// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatters convert Request body items into known Method parameter types.
    /// </summary>
    public interface IInputFormatter
    {
        /// <summary>
        /// String representation of the MIME type accepted
        /// e.g. application/json
        /// </summary>
        public string SupportedMediaType { get; }

        /// <summary>
        /// The <see cref="MediaTypeHeaderValue"/> this input formatter supports.
        /// </summary>
        public MediaTypeHeaderValue SupportedMediaTypeValue { get; }

        /// <summary>
        /// Whether this input formatter supports async calls.
        /// </summary>
        public bool HandlesAsyncFormatting { get; }

        /// <summary>
        /// Converts the request's form body into specified type (asynchronously).
        /// </summary>
        /// <param name="data">The form body data - usually as a string</param>
        /// <param name="type">The type to convert to (deserialise)</param>
        /// <returns></returns>
        public async Task<object> FormatRequestAsync(object data, Type type)
        {
            return await Task.FromResult<object>(null);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the request's form body into specified type (synchronously).
        /// </summary>
        /// <param name="data">The form body data - usually as a string</param>
        /// <param name="type">The type to convert to (deserialise)</param>
        /// <returns></returns>
        public object FormatRequest(object data, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
