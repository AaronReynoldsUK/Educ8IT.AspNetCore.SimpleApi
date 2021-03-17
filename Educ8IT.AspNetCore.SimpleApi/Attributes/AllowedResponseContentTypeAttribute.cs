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
    public class AllowedResponseContentTypeAttribute : Attribute, IAllowedResponseContentTypeInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContentType"></param>
        public AllowedResponseContentTypeAttribute(string ContentType)
        {
            this.ContentType = ContentType;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAllowedResponseContentTypeInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }
    }
}
