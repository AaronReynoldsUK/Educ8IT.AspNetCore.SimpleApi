// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public static class BearerAuthenticationExtensions
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddBearer(
            this AuthenticationBuilder builder)
        => builder.AddBearer(
            BearerAuthenticationDefaults.AuthenticationScheme,
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
        public static AuthenticationBuilder AddBearer(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<BearerAuthenticationOptions> configureOptions)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<BearerAuthenticationOptions>, BearerAuthenticationPostConfigureOptions>();

            //builder.Services.AddScoped<IIdentityManagerService, BearerIdentityManagerService>();

            return builder.AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <typeparam name="TAuthService"></typeparam>
        //        /// <param name="builder"></param>
        //        /// <returns></returns>
        //        public static AuthenticationBuilder AddBearer<TAuthService>(this AuthenticationBuilder builder)
        //            where TAuthService : class, IBearerAuthenticationService
        //        {
        //            return AddBearer<TAuthService>(builder, BearerAuthenticationDefaults.AuthenticationScheme, _ => { });
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <typeparam name="TAuthService"></typeparam>
        //        /// <param name="builder"></param>
        //        /// <param name="authenticationScheme"></param>
        //        /// <returns></returns>
        //        public static AuthenticationBuilder AddBearer<TAuthService>(
        //            this AuthenticationBuilder builder, 
        //            string authenticationScheme)
        //            where TAuthService : class, IBearerAuthenticationService
        //        {
        //            return AddBearer<TAuthService>(builder, authenticationScheme, _ => { });
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <typeparam name="TAuthService"></typeparam>
        //        /// <param name="builder"></param>
        //        /// <param name="configureOptions"></param>
        //        /// <returns></returns>
        //        public static AuthenticationBuilder AddBearer<TAuthService>(
        //            this AuthenticationBuilder builder, 
        //            Action<BearerAuthenticationOptions> configureOptions)
        //            where TAuthService : class, IBearerAuthenticationService
        //        {
        //            return AddBearer<TAuthService>(builder, BearerAuthenticationDefaults.AuthenticationScheme, configureOptions);
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <typeparam name="TAuthService"></typeparam>
        //        /// <param name="builder"></param>
        //        /// <param name="authenticationScheme"></param>
        //        /// <param name="configureOptions"></param>
        //        /// <returns></returns>
        //        public static AuthenticationBuilder AddBearer<TAuthService>(
        //            this AuthenticationBuilder builder, 
        //            string authenticationScheme, 
        //            Action<BearerAuthenticationOptions> configureOptions)
        //            where TAuthService : class, IBearerAuthenticationService
        //        {
        //            builder.Services.AddSingleton<IPostConfigureOptions<BearerAuthenticationOptions>, BearerAuthenticationPostConfigureOptions>();

        //            //builder.Services.AddTransient<IBearerAuthenticationService, TAuthService>();
        //            //builder.Services.AddSingleton<IBearerAuthenticationService, TAuthService>();
        //            //builder.Services.AddSingleton<IAuthenticationService, TAuthService>();
        ////            builder.Services.AddScoped<IBearerAuthenticationService, TAuthService>();
        //            builder.Services.AddScoped<IAuthenticationService, TAuthService>();
        //            //builder.Services.AddSingleton<BearerAuthenticationOptions, BearerAuthenticationOptions>);

        //            return builder.AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(
        //                authenticationScheme, configureOptions);
        //        }


    }
}
