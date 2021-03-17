// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlDocumentationDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SettingsKey = "HtmlDocumentation";

        /// <summary>
        /// 
        /// </summary>
        public const string DocumentationControllerRoute = "htmlDocs";

        /// <summary>
        /// 
        /// </summary>
        public const string DocumentationHomeRoute = "";

        /// <summary>
        /// 
        /// </summary>
        public const string DocumentationByControllerRoute = "[controller]";

        /// <summary>
        /// 
        /// </summary>
        public const string DocumentationByMethodRoute = "[controller]/[method]";

        /// <summary>
        /// 
        /// </summary>
        public const string DocumentationTypeRoute = "type?type=[type]";
    }
}
