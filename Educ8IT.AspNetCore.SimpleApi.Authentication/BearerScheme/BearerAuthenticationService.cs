// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class BearerAuthenticationService : IBearerAuthenticationService
    {
        private readonly ILogger<BearerAuthenticationService> _logger;
        private readonly IAuthenticationHandlerProvider _handlers;
        private readonly IIdentityManagerService _identityManagerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="handlers"></param>
        /// <param name="identityManagerService"></param>
        public BearerAuthenticationService(
            ILogger<BearerAuthenticationService> logger,
            IAuthenticationHandlerProvider handlers,
            IIdentityManagerService identityManagerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var handler = await _handlers.GetHandlerAsync(context, scheme);

            var authResult = await handler.AuthenticateAsync();

            return authResult;

            //var __authResult = AuthenticateResult.NoResult();

            //return Task.FromResult(__authResult);
        }

        /// <summary>
        /// This will instruct your browser on where to go to be authenticated. 
        /// For example redirection to the Auth endpoint.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            //context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// This step takes the ClaimsPrincipal built from the previous step, and persists it.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="principal"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// This is the reverse step of the SignIn step. 
        /// It instructs the middleware to delete any persisted data.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }
    }
}
