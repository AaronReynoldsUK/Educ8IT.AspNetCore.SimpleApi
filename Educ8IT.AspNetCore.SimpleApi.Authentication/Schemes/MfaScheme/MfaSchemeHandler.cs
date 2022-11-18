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

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaSchemeHandler : AuthenticationHandler<MfaSchemeOptions>, ICanAuthenticateRequest
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
        /// <exception cref="ArgumentNullException"></exception>
        public MfaSchemeHandler(
            IOptionsMonitor<MfaSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IIdentityManagerService identityManagerService)
            : base(options, logger, encoder, clock)
        {
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }

        #region AuthenticationHandler

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;
            return AuthenticateResult.NoResult();
        }

        /// <summary>
        /// 
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
                $"{Scheme.Name} ruri=\"{Options.RequestPath}\"");

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
                MfaSchemeDefaults.AuthorisationHeaderName,
                MfaSchemeDefaults.AuthenticationScheme);
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

            var __accessToken = apiUserTokens.Where(t => t.TokenType == MfaSchemeDefaults.VerifyTokenName).FirstOrDefault();
            if (__accessToken != null)
            {
                claimsPrinciple.AddClaim(new Claim(AuthenticationClaimTypes.Mfa, System.Boolean.TrueString));
            }

            await Task.CompletedTask;
        }

        #endregion
    }
}
