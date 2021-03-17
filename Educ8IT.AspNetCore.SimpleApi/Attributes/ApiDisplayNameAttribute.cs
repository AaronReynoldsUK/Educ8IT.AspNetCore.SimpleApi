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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiDisplayNameAttribute : Attribute, IApiDisplayNameInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public ApiDisplayNameAttribute(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IApiDisplayNameInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
    }
}
