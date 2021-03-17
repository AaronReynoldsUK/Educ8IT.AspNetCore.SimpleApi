// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public static class CustomEndpointRouteBuilderExtensions
    {
        //public static IEndpointConventionBuilder MapFramework(
        //    this IEndpointRouteBuilder endpoints, 
        //    Action<CustomConfigurationBuilder> configure)
        //{
        //    if (endpoints == null)
        //    {
        //        throw new ArgumentNullException(nameof(endpoints));
        //    }
        //    if (configure == null)
        //    {
        //        throw new ArgumentNullException(nameof(configure));
        //    }

        //    var dataSource = endpoints.ServiceProvider.GetRequiredService<CustomEndpointDataSource>();

        //    var configurationBuilder = new CustomConfigurationBuilder(dataSource);
        //    configure(configurationBuilder);

        //    endpoints.DataSources.Add(dataSource);

        //    return dataSource;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static IEndpointConventionBuilder MapFramework(
            this IEndpointRouteBuilder endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var dataSource = endpoints.ServiceProvider.GetRequiredService<CustomEndpointDataSource>();

            var configurationBuilder = new CustomConfigurationBuilder(dataSource);
            //configure(configurationBuilder);

            endpoints.DataSources.Add(dataSource);

            return dataSource;
        }
    }
}
