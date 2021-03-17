// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// Identifies an action that supports the HTTP GET method.
    /// </summary>
    public class HttpGetAttribute : HttpMethodAttribute
    {
        private static readonly IEnumerable<string> _supportedMethods = new[] { "GET" };

        /// <summary>
        /// Creates a new <see cref="HttpGetAttribute"/>.
        /// </summary>
        public HttpGetAttribute()
            : base(_supportedMethods)
        {
        }

        /// <summary>
        /// Creates a new <see cref="HttpGetAttribute"/> with the given route template.
        /// </summary>
        /// <param name="routeTemplates">The route template(s). May not be null.</param>
        public HttpGetAttribute(params string[] routeTemplates)
            : base(_supportedMethods, routeTemplates)
        {
            if (routeTemplates == null)
            {
                throw new ArgumentNullException(nameof(routeTemplates));
            }
        }
    }
}
