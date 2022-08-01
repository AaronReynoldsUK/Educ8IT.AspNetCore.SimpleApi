// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationAwareApiMapperServiceExtensions
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

        //public static async Task<EAuthenticationType> Get(
        //    this HttpContext context,
        //    AuthenticationAwareApiMethodItem methodItem)
        //{
        //    var __authenticationServices = context.RequestServices.GetServices<IAuthenticationService>();

        //    if (__authenticationServices == null)
        //        throw new Exception("No Authentication services are available");

        //    var __authenticateAttribute = methodItem.AuthenticateAttribute;
        //    var __authenticationRequired = __authenticateAttribute.AuthenticationTypeRequired;

        //    var __authenticationAchieved = context.User.GetAuthenticationType();
            
        //    foreach (var __authenticationService in __authenticationServices)
        //    {
        //        if ((__authenticationAchieved & __authenticationRequired) == __authenticationRequired)
        //        {
        //            // OK
        //        }
        //        else
        //        {
        //            // need to be able to get the schemes
        //            //context.ChallengeAsync();
        //            //__authenticationService.ChallengeAsync(context, 
        //        }
                
        //        break;
        //    }

        //    return EAuthenticationType.None;
        //}
    }
}
