// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationHandlerOfTypeAuthenticatedRequirement : AuthorisationHandlerGeneric<AuthenticatedRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AuthenticatedRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            if (context.User?.Identity.IsAuthenticated ?? false)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
