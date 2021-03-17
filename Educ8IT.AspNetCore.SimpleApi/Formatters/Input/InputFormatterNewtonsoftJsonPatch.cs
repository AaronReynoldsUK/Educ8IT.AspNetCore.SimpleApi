﻿// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatter for "application/json-patch+json".
    /// Uses the <see cref="Newtonsoft.Json.JsonSerializer"/>
    /// </summary>
    public class InputFormatterNewtonsoftJsonPatch : InputFormatterNewtonsoftJson
    {
        /// <summary>
        /// Set MIME type handled to "application/json-patch+json"
        /// </summary>
        public InputFormatterNewtonsoftJsonPatch()
            : base("application/json-patch+json", false)
        { }

        /// <summary>
        /// Synchronously converts the JSON Patch document (text) into a CLR object.
        /// </summary>
        /// <param name="data">JSON Patch document</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public override object FormatRequest(object data, Type type)
        {
            return base.FormatRequest(data, type);
        }
    }
}
