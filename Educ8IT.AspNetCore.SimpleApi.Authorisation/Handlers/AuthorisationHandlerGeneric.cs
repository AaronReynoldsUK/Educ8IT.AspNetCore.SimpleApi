// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            return Task.CompletedTask;
        }
    }
}
