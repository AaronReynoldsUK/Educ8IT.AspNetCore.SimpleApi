// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Abstract base class for Output Formatters.
    /// Output Formatters serialise the Response (body) for transmission over HTTP.
    /// </summary>
    public abstract class OutputFormatter : IOutputFormatter
    {
        /// <summary>
        /// Default constructor for Output Formatter.
        /// Checks that supportedMediaType is provided and parses into a <see cref="MediaTypeHeaderValue"/>.
        /// </summary>
        /// <param name="supportedMediaType">The MIME type this formatter supports</param>
        /// <param name="handlesAsyncFormatting">Whether this formatter supports asynchronous formatting</param>
        public OutputFormatter(string supportedMediaType, bool handlesAsyncFormatting = false)
        {
            SupportedMediaType = supportedMediaType
                ?? throw new ArgumentNullException(nameof(supportedMediaType));

            if (! MediaTypeHeaderValue.TryParse(supportedMediaType, out MediaTypeHeaderValue mediaTypeHeaderValue))
                throw new ArgumentException("Not a valid media type", nameof(supportedMediaType));

            SupportedMediaTypeValue = mediaTypeHeaderValue;

            HandlesAsyncFormatting = handlesAsyncFormatting;
        }

        /// <inheritdoc/>
        public string SupportedMediaType { get; private set; } = "*/";

        /// <inheritdoc/>
        public MediaTypeHeaderValue SupportedMediaTypeValue { get; private set; }

        /// <inheritdoc/>
        public bool HandlesAsyncFormatting { get; }

        /// <inheritdoc/>
        public virtual async Task<ResponseObject> FormatResponseAsync(ResponseObject responseObject)
        {
            return await Task.FromResult<ResponseObject>(responseObject);
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual ResponseObject FormatResponse(ResponseObject responseObject)
        {
            throw new NotImplementedException();
        }
    }
}
