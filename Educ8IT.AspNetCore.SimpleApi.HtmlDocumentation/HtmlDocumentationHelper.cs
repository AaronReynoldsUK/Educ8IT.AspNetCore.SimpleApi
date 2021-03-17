// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlDocumentationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlDocumentationOptions"></param>
        /// <returns></returns>
        public static string GetRootUri(IHtmlDocumentationOptions htmlDocumentationOptions)
        {
            var __documentationRootUrl = htmlDocumentationOptions.DocumentationControllerRoutes?.FirstOrDefault() ?? "htmlDocs";

            if (__documentationRootUrl == null)
                __documentationRootUrl = String.Empty;

            if (__documentationRootUrl.StartsWith("/"))
                __documentationRootUrl = __documentationRootUrl.TrimStart('/');
            if (__documentationRootUrl.EndsWith("/"))
                __documentationRootUrl = __documentationRootUrl.TrimEnd('/');

            return $"/{__documentationRootUrl}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlDocumentationOptions"></param>
        /// <param name="apiControllerItem"></param>
        /// <returns></returns>
        public static string GetContollerUri(IHtmlDocumentationOptions htmlDocumentationOptions, IApiControllerItem apiControllerItem)
        {
            var __documentationRootUrl = GetRootUri(htmlDocumentationOptions);

            var __subRoute = htmlDocumentationOptions.DocumentationByControllerRoutes?.FirstOrDefault() ?? HtmlDocumentationDefaults.DocumentationByControllerRoute;

            if (__subRoute == null)
                __subRoute = String.Empty;

            __subRoute = __subRoute
                .Replace("[controller]", apiControllerItem.Name);

            if (__subRoute.StartsWith("/"))
                __subRoute = __subRoute.TrimStart('/');
            if (__subRoute.EndsWith("/"))
                __subRoute = __subRoute.TrimEnd('/');

            return $"{__documentationRootUrl}/{__subRoute}".Replace("//", "/");
        }

        /// <summary>
        /// Gets the URI for the documentation for a specified method
        /// </summary>
        /// <param name="htmlDocumentationOptions"></param>
        /// <param name="apiMethodItem"></param>
        public static string GetMethodUri (IHtmlDocumentationOptions htmlDocumentationOptions, IApiMethodItem apiMethodItem)
        {
            var __documentationRootUrl = GetRootUri(htmlDocumentationOptions);

            var __subRoute = htmlDocumentationOptions.DocumentationByMethodRoutes?.FirstOrDefault() ?? HtmlDocumentationDefaults.DocumentationByMethodRoute;

            if (__subRoute == null)
                __subRoute = String.Empty;

            __subRoute = __subRoute
                .Replace("[controller]", apiMethodItem.ApiControllerItem.Name)
                .Replace("[method]", apiMethodItem.Name);

            if (__subRoute.StartsWith("/"))
                __subRoute = __subRoute.TrimStart('/');
            if (__subRoute.EndsWith("/"))
                __subRoute = __subRoute.TrimEnd('/');

            return $"{__documentationRootUrl}/{__subRoute}".Replace("//", "/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlDocumentationOptions"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetTypeUri(IHtmlDocumentationOptions htmlDocumentationOptions, string typeName)
        {
            var __documentationRootUrl = GetRootUri(htmlDocumentationOptions);

            var __subRoute = htmlDocumentationOptions.DocumentationTypeRoutes?.FirstOrDefault() ?? HtmlDocumentationDefaults.DocumentationTypeRoute;

            if (__subRoute == null)
                __subRoute = String.Empty;

            __subRoute = __subRoute
                .Replace("[type]", typeName)
                .Replace("{typeName}", typeName);

            if (__subRoute.StartsWith("/"))
                __subRoute = __subRoute.TrimStart('/');
            if (__subRoute.EndsWith("/"))
                __subRoute = __subRoute.TrimEnd('/');

            return $"{__documentationRootUrl}/{__subRoute}".Replace("//", "/");
        }
    }
}
