// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatter for "application/json".
    /// Uses the <see cref="System.Text.Json.JsonSerializer"/>
    /// </summary>
    public class InputFormatterJson : InputFormatter
    {
        /// <summary>
        /// Set MIME type handled to "application/json"
        /// </summary>
        public InputFormatterJson()
            : base("application/json", true)
        { }

        /// <summary>
        /// Set MIME type handled to "supportedMimeType"
        /// </summary>
        /// <param name="supportedMimeType">Allows sub-setting of the JSON mime-type</param>
        /// <param name="handlesAsyncFormatting">Whether this formatter supports asynchronous formatting</param>
        public InputFormatterJson(string supportedMimeType, bool handlesAsyncFormatting = false)
            : base(supportedMimeType, handlesAsyncFormatting)
        { }

        /// <summary>
        /// Asynchronously converts the Input object (text) from JSON into a CLR object
        /// </summary>
        /// <param name="data">JSON string object</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public async override Task<object> FormatRequestAsync(object data, Type type)
        {
            if (data == null)
                return null;

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            object __result = null;
            var __dataAsString = data.ToString();

            using (var __newInputStream = new MemoryStream())
            {
                var __writer = new StreamWriter(__newInputStream);
                await __writer.WriteAsync(__dataAsString);
                __writer.Flush();
                __newInputStream.Position = 0;

                __result = await JsonSerializer.DeserializeAsync(__newInputStream, type);
            }

            return __result;
        }

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

            return JsonSerializer.Deserialize(__dataAsString, type);
        }
    }
}
