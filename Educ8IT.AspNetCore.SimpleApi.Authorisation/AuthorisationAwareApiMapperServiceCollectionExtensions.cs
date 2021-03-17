using Educ8IT.AspNetCore.SimpleApi.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthorisationAwareApiMapperServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorisationAwareApiMapper(
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
            services.AddSingleton<IApiMapperService, AuthorisationAwareApiMapperService>();
            services.AddSingleton<IApiMapperOptions, ApiMapperOptions>();
            services.AddSingleton<CustomEndpointDataSource>();
        }
    }
}
