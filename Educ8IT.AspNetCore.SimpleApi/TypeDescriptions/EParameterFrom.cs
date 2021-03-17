// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public enum EParameterFrom
    {
        /// <summary>
        /// Reserved for future use
        /// </summary>
        Service,

        /// <summary>
        /// 
        /// </summary>
        Context,

        /// <summary>
        /// 
        /// </summary>
        Header,

        /// <summary>
        /// 
        /// </summary>
        Route,

        /// <summary>
        /// 
        /// </summary>
        Body,

        /// <summary>
        /// 
        /// </summary>
        Form,

        /// <summary>
        /// 
        /// </summary>
        Query
    }
}
