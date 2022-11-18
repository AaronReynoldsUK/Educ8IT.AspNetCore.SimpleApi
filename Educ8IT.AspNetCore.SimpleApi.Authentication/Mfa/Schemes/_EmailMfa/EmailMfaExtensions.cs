using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailMfaExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MfaBuilder AddEmailMfa(this MfaBuilder builder)
            => builder.AddEmailMfa(_ => { });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static MfaBuilder AddEmailMfa(this MfaBuilder builder,
            Action<EmailMfaOptions> configureOptions)
        {
            return builder.AddScheme<EmailMfaOptions, EmailMfaHandler>(
                EmailMfaDefaults.SCHEME_NAME,
                EmailMfaDefaults.SCHEME_DISPLAY_NAME,
                EmailMfaDefaults.METHOD,
                configureOptions);
        }
    }
}
