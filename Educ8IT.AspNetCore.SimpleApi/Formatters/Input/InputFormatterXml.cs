// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// Input Formatter for ""application/xml".
    /// </summary>
    public class InputFormatterXml: InputFormatter
    {
        /// <summary>
        /// Set MIME type handled to "application/xml"
        /// </summary>
        public InputFormatterXml()
            : base("application/xml", false)
        { }

        /// <summary>
        /// Synchronously converts the XML document (text string) into a CLR object.
        /// </summary>
        /// <param name="data">XML document as string</param>
        /// <param name="type">Destination CLR type</param>
        /// <returns>CLR object of specified type</returns>
        public override object FormatRequest(object data, Type type)
        {
            if (data == null)
                return null;

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var __dataAsString = data.ToString();

            return __dataAsString.DeserialiseFromXml(type);
        }
    }
}
