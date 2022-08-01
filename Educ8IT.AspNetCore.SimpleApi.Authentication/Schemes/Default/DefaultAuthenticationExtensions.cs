using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Schemes
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultAuthenticationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddDefaultAuthentication(
            this AuthenticationBuilder builder)
        {
            builder.Services.AddScoped<IAuthenticationService, DefaultAuthenticationService>();
            //builder.Services.AddScoped<IAuthenticationServiceExtended, DefaultAuthenticationService>();

            builder.AddPolicyScheme(
                DefaultAuthenticationDefaults.AuthenticationScheme,
                DefaultAuthenticationDefaults.DisplayName,
                options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var __authHeader = context.GetAuthenticationHeader();
                        if (__authHeader != null)
                            return __authHeader.Scheme;

                        return null;
                    };
                    
                });

            return builder;
        }
    }
}
