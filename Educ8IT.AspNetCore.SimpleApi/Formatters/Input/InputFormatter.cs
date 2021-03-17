// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Abstract base class for Input Formatters.
    /// Input Formatters convert Request body items into known Method parameter types
    /// </summary>
    public abstract class InputFormatter : IInputFormatter
    {
        /// <summary>
        /// Default constructor for Input Formatter.
        /// Checks that supportedMediaType is provided and parses into a <see cref="MediaTypeHeaderValue"/>.
        /// </summary>
        /// <param name="supportedMediaType">The MIME type this formatter supports</param>
        /// <param name="handlesAsyncFormatting">Whether this formatter supports asynchronous formatting</param>
        public InputFormatter(string supportedMediaType, bool handlesAsyncFormatting = false)
        {
            SupportedMediaType = supportedMediaType
                ?? throw new ArgumentNullException(nameof(supportedMediaType));

            if (!MediaTypeHeaderValue.TryParse(supportedMediaType, out MediaTypeHeaderValue mediaTypeHeaderValue))
                throw new ArgumentException("Not a valid media type", nameof(supportedMediaType));

            SupportedMediaTypeValue = mediaTypeHeaderValue;

            HandlesAsyncFormatting = handlesAsyncFormatting;
        }

        /// <inheritdoc/>
        public string SupportedMediaType { get; private set; } = "*/";

        /// <inheritdoc/>
        public MediaTypeHeaderValue SupportedMediaTypeValue { get; private set; }

        /// <inheritdoc/>
        /// <remarks>
        /// Could replace property setting with:
        /// 
        /// get {
        ///     var __thisType = this.GetType();
        ///     return __thisType.GetMethod("FormatRequestAsync").DeclaringType == __thisType
        ///         && !__thisType.GetMethod("FormatRequestAsync").IsAbstract);
        /// }
        /// 
        /// but this uses reflection which unnessarily arduous.
        /// </remarks>
        public bool HandlesAsyncFormatting { get; }
       
        /// <inheritdoc/>
        public virtual async Task<object> FormatRequestAsync(object data, Type type)
        {
            return await Task.FromResult<object>(null);
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual object FormatRequest(object data, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
