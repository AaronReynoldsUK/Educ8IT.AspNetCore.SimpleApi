// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequirement"></typeparam>
    public class AuthorisationHandlerGeneric<TRequirement> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        /// <inheritdoc/>
        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            return base.HandleAsync(context);

            //var pendingRequirements = context.PendingRequirements.ToList();

            //foreach(var requirement in pendingRequirements)
            //{
            //    HandleRequirementAsync(context, requirement);
            //}

        }

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            TRequirement requirement)
        {
            //context.Succeed(requirement);

            return base.HandleAsync(context);
            //return Task.CompletedTask;
        }
    }
}
