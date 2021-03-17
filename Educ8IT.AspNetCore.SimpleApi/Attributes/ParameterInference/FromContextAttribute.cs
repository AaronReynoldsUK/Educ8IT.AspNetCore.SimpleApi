// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// Read value from HttpContext Items
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class FromContextAttribute : Attribute
    {
        /// <summary>
        /// Default Attribute constructor
        /// </summary>
        public FromContextAttribute(string contextItemName)
        {
            ContextItemName = contextItemName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContextItemName { get; set; }
    }
}
