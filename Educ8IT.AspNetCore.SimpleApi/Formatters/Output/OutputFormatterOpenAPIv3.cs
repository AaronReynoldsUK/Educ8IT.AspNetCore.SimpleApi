// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Formatters
{
    /// <summary>
    /// TODO: Provide OpenAPI v3 response
    /// mime-type = application/openapi-v3+json
    /// </summary>
    public class OutputFormatterOpenAPIv3 : OutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public OutputFormatterOpenAPIv3()
            : base("application/openapi-v3+json")
        { }

    }
}
