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

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVerificationHandler : AuthenticationHandler<EmailVerificationOptions>, ICanAuthenticateRequest
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
        public EmailVerificationHandler(
            IOptionsMonitor<EmailVerificationOptions> options,
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

            var __emailClaim = Context.User.GetClaim(ClaimTypes.Email);
            if (__emailClaim == null || String.IsNullOrEmpty(__emailClaim.Value))
                return;

            // Send Response
            Response.StatusCode = 401;
            Response.Headers.AppendToEntry(
                Microsoft.Net.Http.Headers.HeaderNames.WWWAuthenticate,
                $"{Scheme.Name} email=\"{__emailClaim.Value}\" ruri=\"{Options.RequestPath}\"");

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
                EmailVerificationDefaults.AuthorisationHeaderName,
                EmailVerificationDefaults.AuthenticationScheme);
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

            var __confirmationToken = apiUserTokens.Where(t => t.TokenType == EmailVerificationDefaults.VerifyTokenName).FirstOrDefault();
            if (__confirmationToken != null)
            {
                claimsPrinciple.AddClaim(new Claim(AuthenticationClaimTypes.EmailConfirmed, System.Boolean.TrueString));
            }

            await Task.CompletedTask;            
        }

        #endregion
    }
}
