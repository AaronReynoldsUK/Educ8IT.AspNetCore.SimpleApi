using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public static class MfaSchemeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddMfa(
            this AuthenticationBuilder builder
        ) => builder.AddMfa(
            MfaSchemeDefaults.AuthenticationScheme,
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
        public static AuthenticationBuilder AddMfa(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<MfaSchemeOptions> configureOptions
        )
        {
            builder.Services.AddSingleton<IPostConfigureOptions<MfaSchemeOptions>, MfaSchemePostConfigureOptions>();

            //builder.Services.AddScoped<IAuthenticationService, EmailVerificationService>();

            return builder.AddScheme<MfaSchemeOptions, MfaSchemeHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
