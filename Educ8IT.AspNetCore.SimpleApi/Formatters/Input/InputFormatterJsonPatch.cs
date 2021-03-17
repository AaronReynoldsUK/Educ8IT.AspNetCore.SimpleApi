// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatter for "application/json".
    /// Uses the <see cref="System.Text.Json.JsonSerializer"/>
    /// </summary>
    public class InputFormatterJsonPatch : InputFormatterJson
    {
        /// <summary>
        /// Set MIME type handled to "application/json-patch+json"
        /// </summary>
        public InputFormatterJsonPatch()
            : base("application/json-patch+json", false)
        { }

        /// <summary>
        /// Asynchronously converts the JSON Patch document (text) into a CLR object
        /// </summary>
        /// <param name="data">JSON string object</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public async override Task<object> FormatRequestAsync(object data, Type type)
        {
            return await base.FormatRequestAsync(data, type);
        }

        /// <summary>
        /// Synchronously converts the JSON Patch document (text) into a CLR object
        /// </summary>
        /// <param name="data">JSON string object</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public override object FormatRequest(object data, Type type)
        {
            return base.FormatRequest(data, type);
        }
    }
}
