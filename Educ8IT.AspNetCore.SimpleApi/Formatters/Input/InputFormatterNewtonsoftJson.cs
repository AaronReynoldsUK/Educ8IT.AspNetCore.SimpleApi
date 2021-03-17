// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatter for "application/json".
    /// Uses the <see cref="Newtonsoft.Json.JsonSerializer"/>
    /// </summary>
    public class InputFormatterNewtonsoftJson : InputFormatter
    {
        /// <summary>
        /// Set MIME type handled to "application/json"
        /// </summary>
        public InputFormatterNewtonsoftJson()
            : base("application/json", false)
        { }

        /// <summary>
        /// Set MIME type handled to "supportedMimeType"
        /// </summary>
        /// <param name="supportedMimeType">Allows sub-setting of the JSON mime-type</param>
        /// <param name="handlesAsyncFormatting"></param>
        public InputFormatterNewtonsoftJson(string supportedMimeType, bool handlesAsyncFormatting = false)
            : base(supportedMimeType, handlesAsyncFormatting)
        { }

        /// <summary>
        /// Synchronously converts the Input object (text) from JSON into a CLR object
        /// </summary>
        /// <param name="data">JSON string object</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public override object FormatRequest(object data, Type type)
        {
            if (data == null)
                return null;

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var __dataAsString = data.ToString();

            return JsonConvert.DeserializeObject(__dataAsString, type);
        }
    }
}
