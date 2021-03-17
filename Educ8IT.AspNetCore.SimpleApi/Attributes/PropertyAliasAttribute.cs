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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyAliasAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyAliasAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Alias"></param>
        public PropertyAliasAttribute(string Alias)
        {
            this.Alias = Alias;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; }
    }
}
