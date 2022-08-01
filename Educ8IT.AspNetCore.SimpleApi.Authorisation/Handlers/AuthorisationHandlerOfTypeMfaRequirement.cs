// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationHandlerOfTypeMfaRequirement: AuthorisationHandlerGeneric<MfaRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            MfaRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            if (context.Resource is AuthorisationAwareApiMethodItem apiMethodItem)
            { }
            else
            {
                return Task.CompletedTask;
            }

            var __authAttribute = apiMethodItem.AuthoriseAttribute;

            if (__authAttribute == null)
                return Task.CompletedTask;


            if (context.User?.HasClaim(c => c.Type == MfaRequirement.RequirementKey) ?? false)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
