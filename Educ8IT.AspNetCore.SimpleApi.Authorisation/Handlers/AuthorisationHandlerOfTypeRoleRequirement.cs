// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationHandlerOfTypeRoleRequirement : AuthorisationHandlerGeneric<RoleRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            RoleRequirement requirement)
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

            // Role-based Authorisation
            var __authAttribute = apiMethodItem.AuthoriseAttribute;

            if (__authAttribute == null)
                return Task.CompletedTask;

            // User must be a member of at least one of these Roles
            if ((__authAttribute.Roles?.Length ?? 0) > 0)
            {
                foreach (var role in __authAttribute.Roles)
                {
                    if (context.User.IsInRole(role))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }

                if (!context.HasSucceeded)
                    return Task.CompletedTask;
            }

            // User must be a member of each of these Roles
            if ((__authAttribute.RequiredRoles?.Length ?? 0) > 0)
            {
                foreach (var role in __authAttribute.Roles)
                {
                    if (!context.User.IsInRole(role))
                    {
                        context.Fail();
                        break;
                    }
                }

                if (!context.HasSucceeded)
                    return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
