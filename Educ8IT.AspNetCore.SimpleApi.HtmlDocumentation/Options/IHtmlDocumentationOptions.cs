// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    public interface IHtmlDocumentationOptions
    {
        public string BrandName { get; set; }

        public string BrankLink { get; set; }

        public string CopyrightNotice { get; set; }

        public string[] DocumentationControllerRoutes { get; set; }

        public string[] DocumentationHomeRoutes { get; set; }

        public string[] DocumentationByControllerRoutes { get; set; }

        public string[] DocumentationByMethodRoutes { get; set; }

        public string[] DocumentationTypeRoutes { get; set; }
    }
}
