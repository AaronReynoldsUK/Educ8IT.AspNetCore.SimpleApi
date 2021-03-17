// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiMapperServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IApiMapperBuilder AddApiMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            ConfigureDefaultServices(services);
            AddApiMapperServices(services);

            var __builder = new ApiMapperBuilder(services);

            return __builder;

            //services.AddSingleton<IApiMapperService, ApiMapperService>();

            //var __options = new ApiMapperOptions();
            //Action<ApiMapperOptions> __configure = (__options) => { };

            //services.Configure(__configure);

            //var __builder = new ApiMapperBuilder(services);

            ////configure()
            //return __builder;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiMapper(
            this IServiceCollection services,
            Action<IApiMapperOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            
            ConfigureDefaultServices(services);
            AddApiMapperServices(services);

            services.Configure<ApiMapperOptions>(setupAction);

            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        internal static void AddApiMapperServices(IServiceCollection services)
        {
            services.AddSingleton<IApiMapperService, ApiMapperService>();
            services.AddSingleton<IApiMapperOptions, ApiMapperOptions>();
            services.AddSingleton<CustomEndpointDataSource>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        public static void AddDefaultDocumentationProvider(this IApiMapperOptions apiMapperOptions)
        {
            AddDocumentationProvider(apiMapperOptions, Assembly.GetExecutingAssembly());

            AddDocumentationProvider(apiMapperOptions, Assembly.GetEntryAssembly());
        }

        private static void AddDocumentationProvider(IApiMapperOptions apiMapperOptions, Assembly assembly)
        {
            if (assembly == null)
                return;

            var __assemblyDocumentationPath = assembly.Location?.Replace(".dll", ".xml") ?? null;

            if (__assemblyDocumentationPath == null)
                return;

            if (!System.IO.File.Exists(__assemblyDocumentationPath))
                return;

            var __newDocumentationProvider = new XmlDocumentationProvider(assembly, __assemblyDocumentationPath);

            if (apiMapperOptions.DocumentationProviders.Contains(__newDocumentationProvider))
                return;

            apiMapperOptions.DocumentationProviders.Add(__newDocumentationProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
