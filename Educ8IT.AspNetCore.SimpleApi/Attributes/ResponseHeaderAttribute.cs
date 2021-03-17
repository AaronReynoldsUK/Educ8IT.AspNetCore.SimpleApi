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
    public class ResponseHeaderAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="headerValue"></param>
        public ResponseHeaderAttribute(string headerName, string headerValue)
        {
            Name = headerName;
            Value = headerValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; private set; }
    }
}
