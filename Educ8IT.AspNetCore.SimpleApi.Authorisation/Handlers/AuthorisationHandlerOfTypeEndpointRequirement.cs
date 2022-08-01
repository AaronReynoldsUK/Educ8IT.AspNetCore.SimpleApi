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
    public class AuthorisationHandlerOfTypeEndpointRequirement :
        AuthorisationHandlerGeneric<EndpointRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            EndpointRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            if (context.Resource is ApiMethodItem apiMethodItem)
            { }
            else
            {
                return Task.CompletedTask;
            }

            // we may need to check the AuthoriseAttribute on ApiMethodIdem
            // but for now... let's just compare the Unique name because we're on EndpointPolicy

            // TODO: should this be VALUE not TYPE ??
            if (context.User?.HasClaim(c => c.Type == apiMethodItem.UniqueName) ?? false)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
            //return base.HandleRequirementAsync(context, requirement);
        }
    }
}
