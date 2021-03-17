// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// Specifies an attribute route on a controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        private int? _order;

        /// <summary>
        /// Alternate contructor.
        /// Uses the value of "{controller}" as the default route
        /// </summary>
        /// <param name="name"></param>
        public ApiControllerAttribute(string name):
            this(name, "{controller}")
        { }

        /// <summary>
        /// Creates a new <see cref="ApiControllerAttribute"/> with the given route template.
        /// </summary>
        /// <param name="name">The documented name of this Controller</param>
        /// <param name="routeTemplates">The route template. May not be null.</param>
        public ApiControllerAttribute(string name, params string[] routeTemplates)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RouteTemplates = routeTemplates ?? throw new ArgumentNullException(nameof(routeTemplates));
        }

        /// <inheritdoc />
        public string[] RouteTemplates { get; }

        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower order
        /// value are tried first. If an action defines a route by providing an <see cref="IRouteTemplateProvider"/>
        /// with a non <c>null</c> order, that order is used instead of this value. If neither the action nor the
        /// controller defines an order, a default value of 0 is used.
        /// </summary>
        public int Order
        {
            get { return _order ?? 0; }
            set { _order = value; }
        }

        /// <inheritdoc />
        int? IRouteTemplateProvider.Order => _order;

        /// <inheritdoc />
        public string Name { get; }
    }
}
