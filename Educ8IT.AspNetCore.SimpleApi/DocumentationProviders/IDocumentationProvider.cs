// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.DocumentationProviders
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentationProvider
    {
        /// <summary>
        /// The Assembly to document
        /// </summary>
        public Assembly ApiAssembly { get; }

        /// <summary>
        /// The full path to the Documentation file
        /// </summary>
        public string DocumentationPath { get; }

        /// <summary>
        /// Get documentation for Controllers
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(Type type);

        /// <summary>
        /// Get documentation for Methods
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(MethodInfo methodInfo);

        /// <summary>
        /// Get documentation for Members
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(MemberInfo memberInfo);

        /// <summary>
        /// Get documentation for Parameters
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public string GetSummaryOrDescription(ParameterInfo parameterInfo);
    }
}
