using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection LoadEmailServiceOptions(
            this IServiceCollection serviceDescriptors, 
            IConfiguration configuration)
        {
            return serviceDescriptors
                .Configure<EmailServiceOptions>(
                    configuration.GetSection(EmailServiceOptions.SettingsKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IConfigurationSection GetEmailServiceOptions(IConfiguration configuration)
        {
            return configuration.GetSection(EmailServiceOptions.SettingsKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMockEmailService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.LoadEmailServiceOptions(configuration);

            services.AddSingleton<IEmailService, MockEmailService>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMockEmailService(
            this IServiceCollection services,
            Action<EmailServiceOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            services.Configure<EmailServiceOptions>(setupAction);

            services.AddSingleton<IEmailService, MockEmailService>();

            return services;
        }
    }
}
