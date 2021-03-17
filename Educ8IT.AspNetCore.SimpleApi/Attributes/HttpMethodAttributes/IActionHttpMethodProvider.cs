// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActionHttpMethodProvider
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> HttpMethods { get; }
    }
}
