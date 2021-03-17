// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public static class IdentityManagerServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityManager(
            this IServiceCollection services,
            Action<IIdentityManagerOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            services.Configure<IdentityManagerOptions>(setupAction);

            services.AddScoped<IIdentityManagerService, IdentityManagerService>();

            return services;
        }
    }
}
