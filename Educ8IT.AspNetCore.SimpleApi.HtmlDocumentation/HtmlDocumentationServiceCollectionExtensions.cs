// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlDocumentationServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddHtmlDocumentation(
            this IServiceCollection services,
            Action<IHtmlDocumentationOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            services.AddSingleton<IHtmlDocumentationOptions, HtmlDocumentationOptions>();
            
            services.Configure<HtmlDocumentationOptions>(setupAction);

            return services;
        }
    }
}
