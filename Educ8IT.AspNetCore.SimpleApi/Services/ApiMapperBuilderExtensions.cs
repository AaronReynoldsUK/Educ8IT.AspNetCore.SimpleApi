// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiMapperBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IApiMapperBuilder AddApiMapperOptions(
            this IApiMapperBuilder builder,
            Action<ApiMapperOptions> setupAction)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiMapper(this IApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            return builder.UseApiMapper(routes => { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureRoutes"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiMapper(this IApplicationBuilder builder, Action<IRouteBuilder> configureRoutes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureRoutes == null)
                throw new ArgumentNullException(nameof(configureRoutes));

            //var __routes = new RouteBuilder(builder)
            //{
            //    DefaultHandler = builder.
            //};

            //configureRoutes(__routes);

            //__routes.Routes.Insert(0, )

            return builder;//.UseRouter(__routes.Build());
        }
    }
}
