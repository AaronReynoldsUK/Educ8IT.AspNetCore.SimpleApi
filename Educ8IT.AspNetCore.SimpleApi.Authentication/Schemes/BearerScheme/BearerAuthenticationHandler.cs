// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class BearerAuthenticationHandler : SignInAuthenticationHandler<BearerAuthenticationOptions>, ICanAuthenticateRequest
    {
        #region Fields

        private readonly IIdentityManagerService _identityManagerService;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="identityManagerService"></param>
        public BearerAuthenticationHandler(
            IOptionsMonitor<BearerAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IIdentityManagerService identityManagerService)
            : base(options, logger, encoder, clock)
        {
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }

        #region SignInAuthenticationHandler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            await Task.CompletedTask;
        }

        #endregion

        #region SignOutAuthenticationHandler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            await Task.CompletedTask;
        }

        #endregion

        #region AuthenticationHandler

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(BearerAuthenticationDefaults.AuthorisationHeaderName))
            {
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(
                Request.Headers[BearerAuthenticationDefaults.AuthorisationHeaderName],
                out AuthenticationHeaderValue headerValue))
            {
                return AuthenticateResult.NoResult();
            }

            if (!BearerAuthenticationDefaults.AuthenticationScheme.Equals(
                headerValue.Scheme,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var __endpoint = Context.GetEndpoint();
            if (__endpoint == null)
            {
                return AuthenticateResult.NoResult();
            }

            // Check if the ticket is valid in the DB
            var apiUserToken = await _identityManagerService.GetTokenByTokenValueAsync(
                headerValue.Parameter,
                BearerAuthenticationDefaults.AccessTokenName);

            if (apiUserToken == null)
            {
                return AuthenticateResult.Fail("No such token");
            }

            if (apiUserToken.IsExpired)
            {
                return AuthenticateResult.Fail("expired");
            }

            // Check Extended Data
            if (apiUserToken.HasExtendedData)
            {
                if (apiUserToken.ExtendedData.ContainsKey(BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp))
                {
                    string __value = apiUserToken.ExtendedData[BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp]?.ToString() ?? "";
                    if (!String.Equals(__value,
                        Context.Connection.RemoteIpAddress.ToString(),
                         StringComparison.InvariantCultureIgnoreCase))
                    {
                        return AuthenticateResult.Fail("ip-filter");
                    }
                }
            }

            // Get the User (Principle) + Claims/Roles
            ApiUser __authenticatedUser = await _identityManagerService.GetUserByIdAsync(
                apiUserToken.UserId);

            if (__authenticatedUser == null)
            {
                return AuthenticateResult.Fail("user");
            }

            // Prepare Principle
            var __claimsPrinciple = _identityManagerService.GetClaimsPrinciple(
                __authenticatedUser,
                Scheme.Name);

            if (__claimsPrinciple == null)
            {
                return AuthenticateResult.Fail("claims_principle");
            }

            var __authProperties = new AuthenticationProperties();
            __authProperties.SetParameter(BearerAuthenticationDefaults.AccessTokenName, apiUserToken);

            // Prep for AuthenticateResult
            var authenticationTicket = new AuthenticationTicket(
                __claimsPrinciple,
                __authProperties,
                Scheme.Name);

            if (authenticationTicket == null)
                return AuthenticateResult.Fail("Invalid");

            return AuthenticateResult.Success(authenticationTicket);
        }

        /// <summary>
        /// Update the Response for a Challenge
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var __requiredClaimType = properties.GetRequiredClaim();
            if (__requiredClaimType == null)
                return;

            if (Options.ClaimsHandled == null)
                return;

            if (!Options.ClaimsHandled.Contains(__requiredClaimType))
                return;

            // Send Response
            Response.StatusCode = 401;
            Response.Headers.AppendToEntry(
                Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate,
                $"{Scheme.Name} url=\"{Options.LoginPath}\"");

            await Task.CompletedTask;
        }

        #endregion

        #region ICanAuthenticateRequest

        /// <inheritdoc />
        public bool CanAuthenticateRequest(HttpContext context)
        {
            if (context == null)
                return false;

            return context.CanAuthenticateRequest(
                BearerAuthenticationDefaults.AuthorisationHeaderName,
                BearerAuthenticationDefaults.AuthenticationScheme);
        }

        /// <inheritdoc/>
        public async Task UpdateClaimsAsync(ClaimsPrincipal claimsPrinciple, ApiUser apiUser, List<ApiUserToken> apiUserTokens)
        {
            if (claimsPrinciple == null)
            {
                throw new ArgumentNullException(nameof(claimsPrinciple));
            }
            if (apiUser == null)
            {
                throw new ArgumentNullException(nameof(apiUser));
            }
            if (apiUserTokens == null)
            {
                throw new ArgumentNullException(nameof(apiUserTokens));
            }

            var __accessToken = apiUserTokens.Where(t => t.TokenType == BearerAuthenticationDefaults.AccessTokenName).FirstOrDefault();
            if (__accessToken != null)
            {
                claimsPrinciple.AddClaim(new Claim(AuthenticationClaimTypes.Authenticated, System.Boolean.TrueString));
            }

            await Task.CompletedTask;
        }

        #endregion

        #region AuthenticationHandler overrides

        ///// <summary>
        ///// The handler calls methods on the events which give the application control at certain points where processing is occurring.
        ///// If it is not provided a default instance is supplied which does nothing when the methods are called.
        ///// </summary>
        //protected new BearerAuthenticationEvents Events
        //{
        //    get { return (BearerAuthenticationEvents)base.Events!; }
        //    set { base.Events = value; }
        //}

        ///// <summary>
        ///// Initializes the events object, called once per request by super-class.
        ///// Will call <see cref="CreateEventsAsync()"/>
        ///// </summary>
        ///// <returns></returns>
        //protected override Task InitializeEventsAsync()
        //{
        //    return base.InitializeEventsAsync();
        //}

        ///// <summary>
        ///// Creates a new instance of the events instance.
        ///// </summary>
        ///// <returns>A new instance of the events instance.</returns>
        //protected override Task<object> CreateEventsAsync() => 
        //    Task.FromResult<object>(new BearerAuthenticationEvents());


        ///// <summary>
        ///// Called after options/events have been initialized for the handler to finish initializing itself.
        ///// </summary>
        ///// <returns></returns>
        //protected override Task InitializeHandlerAsync()
        //{
        //    return base.InitializeHandlerAsync();
        //}

        #endregion

    }
}
