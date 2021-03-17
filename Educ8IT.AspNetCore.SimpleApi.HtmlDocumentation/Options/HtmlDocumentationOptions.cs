// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlDocumentationOptions : IHtmlDocumentationOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SettingsKey = "HtmlDocumentation";

        /// <summary>
        /// 
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BrankLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CopyrightNotice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DocumentationControllerRoutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DocumentationHomeRoutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DocumentationByControllerRoutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DocumentationByMethodRoutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DocumentationTypeRoutes { get; set; }
    }
}
