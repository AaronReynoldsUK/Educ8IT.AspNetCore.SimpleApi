// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyValuePairDescription : TypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public TypeDescription KeyDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeDescription ValueDescription { get; set; }
    }
}
