using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.Extensions.Logging;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVerificationService : IAuthenticationService
    {
        private readonly ILogger<EmailVerificationService> _logger;
        private readonly IAuthenticationHandlerProvider _handlers;
        private readonly IIdentityManagerService _identityManagerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="handlers"></param>
        /// <param name="identityManagerService"></param>
        public EmailVerificationService(
            ILogger<EmailVerificationService> logger, 
            IAuthenticationHandlerProvider handlers, 
            IIdentityManagerService identityManagerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }

        /// <summary>
        /// Authenticate for the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <returns>The result.</returns>
        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var handler = await _handlers.GetHandlerAsync(context, scheme);

            var authResult = await handler.AuthenticateAsync();

            return authResult;

            //return Task.FromResult(AuthenticateResult.NoResult());
        }

        /// <summary>
        /// Challenge the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
        /// <returns>A task.</returns>
        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Forbids the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
        /// <returns>A task.</returns>
        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sign a principal in for the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to sign in.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
        /// <returns>A task.</returns>
        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sign out the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="scheme">The name of the authentication scheme.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
        /// <returns>A task.</returns>
        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }
    }
}
