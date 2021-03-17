// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//using Educ8IT.AspNetCore.SimpleApi.NoMvc;
using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// Identifies an action that supports a given set of HTTP methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class HttpMethodAttribute : Attribute, IActionHttpMethodProvider, IRouteTemplateProvider
    {
        private int? _order;

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods.
        /// <param name="httpMethods">The set of supported HTTP methods. May not be null.</param>
        /// </summary>
        public HttpMethodAttribute(IEnumerable<string> httpMethods)
            : this(httpMethods, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods an the given route template.
        /// </summary>
        /// <param name="httpMethods">The set of supported methods. May not be null.</param>
        /// <param name="routeTemplates">The route template.</param>
        public HttpMethodAttribute(IEnumerable<string> httpMethods, params string[] routeTemplates)
        {
            HttpMethods = httpMethods ?? throw new ArgumentNullException(nameof(httpMethods));
            RouteTemplates = routeTemplates;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> HttpMethods { get; }

        /// <summary>
        /// 
        /// </summary>
        public string[] RouteTemplates { get; }

        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first.
        /// </summary>
        public int Order
        {
            get { return _order ?? 0; }
            set { _order = value; }
        }

        /// <inheritdoc />
        public string Name { get; set; }

        int? IRouteTemplateProvider.Order => _order;
    }
}
