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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExcludeFromDocumentationAttribute : Attribute, IExcludeFromDocumentationInterface
    {   
        /// <summary>
        /// 
        /// </summary>
        public ExcludeFromDocumentationAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShouldExclude"></param>
        public ExcludeFromDocumentationAttribute(bool ShouldExclude = true)
        {
            this.ShouldExclude = ShouldExclude;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShouldExclude { get; set; } = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IExcludeFromDocumentationInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ShouldExclude { get; }
    }
}
