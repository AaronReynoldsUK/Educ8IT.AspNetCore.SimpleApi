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
    public class ApiMapperBuilder : IApiMapperBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        public Action<IApiMapperOptions> SetupAction { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ApiMapperBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public ApiMapperBuilder(IServiceCollection services, Action<IApiMapperOptions> setupAction)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            SetupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));
        }
    }
}
