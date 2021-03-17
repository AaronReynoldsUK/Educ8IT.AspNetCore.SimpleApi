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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class FromHeaderAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        public FromHeaderAttribute(string headerName)
        {
            this.Name = headerName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
