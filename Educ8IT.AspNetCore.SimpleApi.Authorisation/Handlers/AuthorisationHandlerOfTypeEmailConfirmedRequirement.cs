// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationHandlerOfTypeEmailConfirmedRequirement : AuthorisationHandlerGeneric<EmailConfirmedRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmailConfirmedRequirement requirement)
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

            if (context.User?.HasClaim(c => c.Type == EmailConfirmedRequirement.RequirementKey) ?? false)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
