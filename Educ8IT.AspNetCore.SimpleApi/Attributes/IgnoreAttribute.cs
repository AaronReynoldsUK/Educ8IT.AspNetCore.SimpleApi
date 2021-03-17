// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ShouldIgnore { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public IgnoreAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shouldIgnore"></param>
        public IgnoreAttribute(bool shouldIgnore = true)
        {
            ShouldIgnore = shouldIgnore;
        }
    }
}
