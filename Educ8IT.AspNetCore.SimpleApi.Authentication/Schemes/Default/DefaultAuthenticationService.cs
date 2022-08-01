using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAuthenticationService : IAuthenticationServiceExtended
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="handlers"></param>
        /// <param name="identityManagerService"></param>
        /// <param name="transform"></param>
        public DefaultAuthenticationService(
            ILogger<DefaultAuthenticationService> logger,
            IAuthenticationHandlerProvider handlers,
            IIdentityManagerService identityManagerService,
            IClaimsTransformation transform)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));
        }

        #endregion

        #region Fields

        private readonly ILogger<DefaultAuthenticationService> _logger;
        private readonly IAuthenticationHandlerProvider _handlers;
        private readonly IIdentityManagerService _identityManagerService;
        private readonly IClaimsTransformation _transform;

        #endregion

        #region Properties

        #endregion

        #region IAuthenticationService

        /// <summary>
        /// Get the authentication data for a request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async Task<AuthenticateResult> IAuthenticationService.AuthenticateAsync(
            HttpContext context,
            string scheme)
        {
            //AuthenticateResult authenticateResult = AuthenticateResult.NoResult();

            //if (this._handlers is AuthenticationHandlerProvider __authHandlersProvider)
            //{
            //    var __schemes = await __authHandlersProvider.Schemes.GetAllSchemesAsync();

            //    foreach (var __scheme in __schemes)
            //    {
            //        if (__scheme.HandlerType == typeof(PolicySchemeHandler))
            //            continue;

            //        if (__scheme.Name == scheme)
            //            continue;

            //        var __authHandler = await __authHandlersProvider.GetHandlerAsync(context, __scheme.Name) as ICanAuthenticateRequest;

            //        if (__authHandler == null)
            //            continue;

            //        if (__authHandler.CanAuthenticateRequest(context))
            //        {
            //            var __tmpAuthResult = await __authHandler.AuthenticateAsync();

            //            if (__tmpAuthResult != null && __tmpAuthResult.Succeeded)
            //            {
            //                authenticateResult = __tmpAuthResult;
            //            }
            //        }
            //    }
            //}

            //return AuthenticateResult.NoResult();


            var __authHeader = context.GetAuthenticationHeader();
            if (__authHeader == null)
                return AuthenticateResult.NoResult();

            if (scheme == null || scheme == Schemes.DefaultAuthenticationDefaults.AuthenticationScheme)
            {
                scheme = __authHeader.Scheme;
            }

            var __authHandler = await _handlers.GetHandlerAsync(context, scheme);
            if (__authHandler == null)
                return AuthenticateResult.NoResult();

            var __authResult = await __authHandler.AuthenticateAsync();
            if (__authResult != null && __authResult.Succeeded)
            {
                await this.UpdateClaimsAsync(context, __authResult.Ticket.Principal);

                var __transformed = await _transform.TransformAsync(__authResult.Principal);
                return AuthenticateResult.Success(
                    new AuthenticationTicket(__transformed, __authResult.Properties, __authResult.Ticket.AuthenticationScheme));
            }

            return __authResult ?? AuthenticateResult.NoResult();
        }

        /// <summary>
        /// Challenge an unauthenticated request
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async Task IAuthenticationService.ChallengeAsync(
            HttpContext context,
            string scheme,
            AuthenticationProperties properties)
        {
            if (this._handlers is AuthenticationHandlerProvider __authHandlersProvider)
            {
                var __schemes = await __authHandlersProvider.Schemes.GetAllSchemesAsync();

                foreach (var __scheme in __schemes)
                {
                    if (__scheme.HandlerType == typeof(PolicySchemeHandler))
                        continue;

                    if (__scheme.Name == scheme)
                        continue;

                    var __handler = await __authHandlersProvider.GetHandlerAsync(context, __scheme.Name);
                    await __handler.ChallengeAsync(properties);
                }
            }

            return;
        }

        /// <summary>
        /// Used when an authenticated request needs to be denied
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async Task IAuthenticationService.ForbidAsync(
            HttpContext context,
            string scheme,
            AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            context.Response.StatusCode = 403;
        }

        /// <summary>
        /// Associate a ClaimsPrinciple
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="principal"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async Task IAuthenticationService.SignInAsync(
            HttpContext context,
            string scheme,
            ClaimsPrincipal principal,
            AuthenticationProperties properties)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (String.IsNullOrEmpty(scheme))
                return;

            var __handler = await _handlers.GetHandlerAsync(context, scheme);
            if (__handler == null)
            {
                return;
            }

            if (!(__handler is IAuthenticationSignInHandler __signInHandler))
            {
                return;
            }

            await __signInHandler.SignInAsync(principal, properties);
        }

        /// <summary>
        /// Remove any associated authentication data
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async Task IAuthenticationService.SignOutAsync(
            HttpContext context,
            string scheme,
            AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        #endregion

        #region IAuthenticationServiceExtended

        /// <inheritdoc/>
        public async Task UpdateClaimsAsync(HttpContext httpContext, ClaimsPrincipal claimsPrinciple)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (claimsPrinciple == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var __claimUserEmail = claimsPrinciple.GetClaim(ClaimTypes.Email);

            if (__claimUserEmail == null || String.IsNullOrEmpty(__claimUserEmail.Value))
                return;

            var __user = await _identityManagerService.GetUserByEmailAddressAsync(__claimUserEmail.Value);
            if (__user == null)
                return;

            var __userTokens = await _identityManagerService.GetTokensByUserIdAsync(__user.Id);
            if (__userTokens == null)
                return;

            if (this._handlers is AuthenticationHandlerProvider __authHandlersProvider)
            {
                var __schemes = await __authHandlersProvider.Schemes.GetAllSchemesAsync();

                foreach (var __scheme in __schemes)
                {
                    if (__scheme.HandlerType == typeof(PolicySchemeHandler))
                        continue;

                    var __handler = await __authHandlersProvider.GetHandlerAsync(httpContext, __scheme.Name);

                    if (__handler is ICanAuthenticateRequest __authHandler)
                    {
                        await __authHandler.UpdateClaimsAsync(claimsPrinciple, __user, __userTokens);
                    }
                }
            }
        }

        #endregion
    }
}
