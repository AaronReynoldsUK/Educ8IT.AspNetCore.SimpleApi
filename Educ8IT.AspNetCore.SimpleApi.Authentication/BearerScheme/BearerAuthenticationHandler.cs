// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme.Model;
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
    public class BearerAuthenticationHandler : SignInAuthenticationHandler<BearerAuthenticationOptions>

    {
        //private readonly IBearerAuthenticationService _authenticationService;
        private readonly IIdentityManagerService _identityManagerService;

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
            //IBearerAuthenticationService authenticationService,
            IIdentityManagerService identityManagerService)
            : base(options, logger, encoder, clock)
        {
            //_authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }
                

        /// <summary>
        /// 
        /// </summary>
        protected new BearerAuthenticationEvents Events
        {
            get { return (BearerAuthenticationEvents)base.Events!; }
            set { base.Events = value; }
        }

        /// <summary>
        /// Creates a new instance of the events instance.
        /// </summary>
        /// <returns>A new instance of the events instance.</returns>
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new BearerAuthenticationEvents());

        private async Task<AuthenticationTicket> GetAuthenticationTicket(
            string bearerToken, AuthenticationProperties properties = null, int? refreshUsingTTL = null)
        {
            var __principal = await GetClaimsPrincipal(bearerToken, refreshUsingTTL);

            if (__principal == null)
                return null;

            var __ticket = new AuthenticationTicket(__principal, properties, Scheme.Name);

            return __ticket;
        }

        private async Task<ClaimsPrincipal> GetClaimsPrincipal(string bearerToken, int? refreshUsingTTL = null)
        {
            ApiUser __authenticatedUser =
                await _identityManagerService.AuthenticateTokenAsync(
                    bearerToken,
                    BearerAuthenticationDefaults.AuthenticationScheme,
                    refreshUsingTTL);

            if (__authenticatedUser == null)
                return null;

            var __claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, __authenticatedUser.UserName)
            };
            __claimsList.AddRange(__authenticatedUser.Claims);
            __claimsList.AddRange(__authenticatedUser.RolesAsClaims);

            var __identity = new ClaimsIdentity(__claimsList, Scheme.Name);
            var __principle = new ClaimsPrincipal(__identity);

            return __principle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(BearerAuthenticationDefaults.AuthorisationHeaderName))
                return AuthenticateResult.NoResult();

            if (!AuthenticationHeaderValue.TryParse(
                Request.Headers[BearerAuthenticationDefaults.AuthorisationHeaderName], 
                out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            if (!BearerAuthenticationDefaults.AuthenticationScheme.Equals(
                headerValue.Scheme, 
                StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            int? refreshUsingTTL = default;
            var __endpoint = Context.GetEndpoint();
            if (__endpoint == null)
            {
                return AuthenticateResult.NoResult();
            }

            if (!__endpoint.DisplayName?.EndsWith("Logout") ?? false)
            {
                refreshUsingTTL = Options.TokenTTL;
            }

            var authenticationTicket = await GetAuthenticationTicket(headerValue.Parameter, null, refreshUsingTTL);

            if (authenticationTicket == null)
                return AuthenticateResult.Fail("Invalid");

            var context = new BearerValidatePrincipleContext(Context, Scheme, Options, authenticationTicket);
            await Events.ValidatePrincipal(context);

            if (context.Principal == null)
            {
                return AuthenticateResult.Fail("No principal.");
            }

            return AuthenticateResult.Success(new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name));
            //return AuthenticateResult.Success(authenticationTicket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            properties ??= new AuthenticationProperties();

            // Obtain new Bearer Token
            var __apiBearerToken = properties.GetParameter<ApiBearerToken>(BearerAuthenticationDefaults.AuthenticationScheme);

            ApiUserToken apiUserToken = new ApiUserToken()
            {
                Id = Guid.NewGuid(),
                Token = __apiBearerToken.BearerToken,
                TokenType = BearerAuthenticationDefaults.AuthenticationScheme,
                UserId = __apiBearerToken.UserId,
                ValidFrom = __apiBearerToken.ValidFromDT,
                ValidUntil = __apiBearerToken.ExpiresDT
            };

            properties.IssuedUtc = __apiBearerToken.ValidFromDT;
            properties.ExpiresUtc = __apiBearerToken.ExpiresDT;

            await _identityManagerService.StoreToken(apiUserToken);

            var signInContext = new BearerSigningInContext(Context, Scheme, Options, user, properties);

            DateTimeOffset issuedUtc;
            if (signInContext.Properties.IssuedUtc.HasValue)
            {
                issuedUtc = signInContext.Properties.IssuedUtc.Value;
            }
            else
            {
                issuedUtc = Clock.UtcNow;
                signInContext.Properties.IssuedUtc = issuedUtc;
            }

            if (!signInContext.Properties.ExpiresUtc.HasValue)
            {
                signInContext.Properties.ExpiresUtc = issuedUtc.AddSeconds(Options.TokenTTL);
            }

            await Events.SigningIn(signInContext);

            var ticket = new AuthenticationTicket(
                signInContext.Principal!,
                signInContext.Properties,
                signInContext.Scheme.Name);

            var __principal = await GetClaimsPrincipal(__apiBearerToken.BearerToken);

            ticket = new AuthenticationTicket(__principal, null, Scheme.Name);

            var __singnedInContext = new BearerSignedInContext(
                Context,
                Scheme,
                signInContext.Principal!,
                signInContext.Properties,
                Options);

            await Events.SignedIn(__singnedInContext);

            Logger.AuthenticationSchemeSignedIn(Scheme.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            properties = properties ?? new AuthenticationProperties();

            var __context = new BearerSigningOutContext(Context, Scheme, Options, properties);

            await Events.SigningOut(__context);

            var __bearerToken = properties.GetParameter<string>(BearerAuthenticationDefaults.AuthenticationScheme);

            await _identityManagerService.RemoveToken(__bearerToken, BearerAuthenticationDefaults.AuthenticationScheme);

            Logger.AuthenticationSchemeSignedOut(Scheme.Name);
        }
    }
}
