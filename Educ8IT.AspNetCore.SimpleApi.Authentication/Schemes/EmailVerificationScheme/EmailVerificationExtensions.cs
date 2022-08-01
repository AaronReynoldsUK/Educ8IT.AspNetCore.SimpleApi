using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailVerificationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddEmailVerification(
            this AuthenticationBuilder builder
        ) => builder.AddEmailVerification(
            EmailVerificationDefaults.AuthenticationScheme,
            displayName: null,
            _ => { });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="displayName"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddEmailVerification(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<EmailVerificationOptions> configureOptions
        )
        {
            builder.Services.AddSingleton<IPostConfigureOptions<EmailVerificationOptions>, EmailVerificationPostConfigureOptions>();

            //builder.Services.AddScoped<IAuthenticationService, EmailVerificationService>();

            return builder.AddScheme<EmailVerificationOptions, EmailVerificationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
