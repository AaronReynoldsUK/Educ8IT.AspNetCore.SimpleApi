// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    /// <summary>
    /// 
    /// </summary>
    public static class IApiMapperOptionsExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        public static void AddHtmlDocumentation(this IApiMapperOptions apiMapperOptions)
        {
            apiMapperOptions.DocumentationProviders.Add(
                new XmlDocumentationProvider(
                    Assembly.GetExecutingAssembly(),
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    String.Format("{0}.xml", Assembly.GetExecutingAssembly().GetName().Name))
                    ));
        }
    }
}
