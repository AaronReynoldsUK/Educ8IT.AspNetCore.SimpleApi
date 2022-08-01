// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthorisationAwareApiMapperServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAuthenticated(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodItem"></param>
        /// <returns></returns>
        public static async Task<AuthorizationResult> AuthorisationCheck(
            this HttpContext context, 
            AuthorisationAwareApiMethodItem methodItem)
        {
            var __authServices = context.RequestServices.GetServices<IAuthorizationService>();
            var __authPolicyProvider = context.RequestServices.GetService<IAuthorizationPolicyProvider>();
            var __defaultPolicy = await __authPolicyProvider.GetDefaultPolicyAsync();

            if (__authServices == null)
                throw new Exception("No Authorisation services are available");

            var __authoriseAttribute = methodItem.AuthoriseAttribute;
            AuthorizationResult authorizationResult = null;

            foreach (var __authService in __authServices)
            {
                if ((__authoriseAttribute.Policies?.Length ?? 0) > 0)
                {
                    foreach (string __authPolicy in __authoriseAttribute.Policies)
                    {
                        authorizationResult = await __authService.AuthorizeAsync(
                            context.User,
                            methodItem,
                            __authPolicy);
                    }
                }
                else if (__defaultPolicy == null)
                {
                    throw new CustomHttpException("No default Authorisation policy defined", 
                        HttpStatusCode.InternalServerError);
                }
                else
                {
                    authorizationResult = await __authService.AuthorizeAsync(
                            context.User,
                            methodItem,
                        __defaultPolicy);
                }
            }

            //context.GetTokenAsync

            return authorizationResult ?? AuthorizationResult.Failed();
        }

    }
}
