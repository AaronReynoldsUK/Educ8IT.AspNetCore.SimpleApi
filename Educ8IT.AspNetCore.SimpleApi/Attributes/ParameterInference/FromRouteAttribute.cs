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
    public class FromRouteAttribute : Attribute
    {
        /// <summary>
        /// Default Attribute constructor
        /// </summary>
        public FromRouteAttribute() { }

        /// <summary>
        /// Alternate constructor where the parameter supplied has a different name to the method's parameter
        /// </summary>
        /// <param name="alias"></param>
        public FromRouteAttribute(string alias)
        {
            this.Alias = alias;
        }

        /// <summary>
        /// The parameter name sent to the API
        /// </summary>
        public string Alias { get; set; }
    }
}