// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiMapperBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        Action<IApiMapperOptions> SetupAction { get; }
    }
}
