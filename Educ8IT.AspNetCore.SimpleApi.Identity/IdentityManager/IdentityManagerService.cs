// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Identity;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class IdentityManagerService : IIdentityManagerService
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IIdentityManagerOptions _options;
        private readonly IdentityDbContext _identityDbContext;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool HasDbContext
        {
            get
            {
                return _identityDbContext != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IdentityDbContext GetIdentityDbContext
        {
            get { return _identityDbContext; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IIdentityManagerOptions Options
        {
            get { return _options; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="options"></param>
        /// <param name="identityDbContext"></param>
        public IdentityManagerService(
            ILoggerFactory loggerFactory,
            IOptions<IdentityManagerOptions> options,
            IdentityDbContext identityDbContext)
        {
            _logger = loggerFactory?.CreateLogger(this.GetType().FullName);

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        }

        #endregion

        

        

       

        /// <inheritdoc/>
        public async Task<bool> IsValidPasswordAsync(ApiUser apiUser, string password)
        {
            bool result = false;

            if (apiUser == null)
                return result;

            if (String.IsNullOrEmpty(apiUser.PasswordHash))
                return result;

            if (String.IsNullOrEmpty(password))
                return result;

            var passwordHasher = new PasswordHasher<ApiUser>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(apiUser, apiUser.PasswordHash, password);

            switch (passwordVerificationResult)
            {
                case PasswordVerificationResult.Failed:
                    await RecordFailedAuthentication(apiUser);
                    result = false;
                    break;
                case PasswordVerificationResult.Success:
                    result = true;
                    break;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdatePasswordHash(apiUser, password);
                    result = true;
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> CanSignInAsync(ApiUser apiUser)
        {
            if (apiUser == null)
                return false;

            if (apiUser.LockOutExpired)
            {
                var __user = await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.Id == apiUser.Id);

                if (__user != null)
                {
                    __user.LockoutUntil = null;
                    __user.AccessFailedAttemptsCurrent = 0;
                    await _identityDbContext.SaveChangesAsync();
                }
            }

            return apiUser.Enabled && !apiUser.IsLockedOut;
        }

        /// <inheritdoc/>
        public async Task RecordFailedAuthentication(ApiUser apiUser)
        {
            if (apiUser == null)
                return;

            var __user = await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.Id == apiUser.Id);

            if (__user != null)
            {
                __user.AccessFailedAttemptsCurrent++;
                __user.AccessFailedAttemptsTotal++;

                if (_options.LockoutAfterNFailedAttempts > 0
                    && __user.AccessFailedAttemptsCurrent >= _options.LockoutAfterNFailedAttempts)
                {
                    __user.LockoutUntil = DateTime.UtcNow.Add(_options.LockoutFor);
                }
                await _identityDbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task ResetFailedAuthentication(ApiUser apiUser)
        {
            if (apiUser == null)
                return;

            var __user = await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.Id == apiUser.Id);

            if (__user != null)
            {
                __user.AccessFailedAttemptsCurrent = 0;
                await _identityDbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public Task<string> GenerateNewPassword(int length = 0)
        {
            if (_options == null)
                throw new Exception("Unable to obtain Identity Manager options");

            if (length == 0)
                length = _options.GeneratedPasswordLength;

            var __password = Passwords.GeneratePassword(length,
                _options.GeneratedPasswordsIncludeLowerCase,
                _options.GeneratedPasswordsIncludeNumbers,
                _options.GeneratedPasswordsIncludeSymbols,
                _options.GeneratedPasswordsIncludeUpperCase);

            return Task.FromResult(__password);
        }

        

        /// <inheritdoc/>
        public async Task UpdatePasswordHash(ApiUser apiUser, string password)
        {
            if (apiUser == null)
                return;

            var passwordHasher = new PasswordHasher<ApiUser>();

            var hashedPassword = passwordHasher.HashPassword(apiUser, password);

            var __user = await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.Id == apiUser.Id);

            if (__user != null)
            {
                __user.PasswordHash = hashedPassword;
                await _identityDbContext.SaveChangesAsync();

                apiUser = __user;
            }

            return;
        }

        

        

        

        

        

        ///// <inheritdoc/>
        //public async Task<ApiUserToken> RefreshAccessToken(ApiUser apiUser, string refreshToken, string tokenType)
        //{
        //    if (apiUser == null)
        //        return null;

        //    var __currentRefreshToken = await _identityDbContext.ApiUserTokens
        //        // Try to prevent cross-overs
        //        .OrderByDescending(t => t.ValidFrom)
        //        .FirstOrDefaultAsync(t =>
        //            t.Id == apiUser.Id
        //            &&
        //            (String.Compare(t.TokenType, tokenType, true) == 0));

        //    if (__currentRefreshToken == null || __currentRefreshToken.IsExpired)
        //    {
        //        // can't refresh, must authenticate again

        //        // Wipe all previous tokens of type
        //        await this.RemoveUserTokens(apiUser, tokenType);
        //        return null;
        //    }
        //    else if (__currentRefreshToken.Token == refreshToken)
        //    {

        //    }

        //    // Wipe all previous tokens of type
        //    await this.RemoveUserTokens(apiUser, tokenType);


        //}

        /// <inheritdoc/>
        public async Task<ApiUser> RegisterAsync(string emailAddress, string password, string displayName = null)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return null;

            if (String.IsNullOrEmpty(password))
                return null;

            emailAddress = emailAddress.ToLowerInvariant();

            ApiUser apiUser = new ApiUser()
            {
                DisplayName = displayName,
                EmailAddress = emailAddress,
                Enabled = true,
                Id = Guid.NewGuid(),
                PasswordHash = Passwords.GeneratePassword(100, true, true, true, true),   // create dummy hash
                UserName = emailAddress
            };

            var __entity = await _identityDbContext.ApiUsers.AddAsync(apiUser);
            await _identityDbContext.SaveChangesAsync();

            var __authUser = __entity.Entity;

            await UpdatePasswordHash(__authUser, password);

            return __authUser;
        }

        /// <inheritdoc/>
        public virtual ClaimsPrincipal GetClaimsPrinciple(ApiUser apiUser, string schemeName, List<Claim> extraClaims = null)
        {
            var __claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, apiUser.DisplayName),
                new Claim(ClaimTypes.Email, apiUser.EmailAddress)
            };

            if (extraClaims != null)
                __claimsList.AddRange(extraClaims);

            __claimsList.AddRange(apiUser.Claims);
            __claimsList.AddRange(apiUser.RolesAsClaims);

            // Get Identity + Principle
            var __identity = new ClaimsIdentity(__claimsList, schemeName);
            var __principle = new ClaimsPrincipal(__identity);

            return __principle;
        }

        /// <inheritdoc/>
        public async Task UpdateClaims(ClaimsPrincipal user, ApiUserToken apiUserToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (apiUserToken == null)
            {
                return;
            }

            // Get current User Tokens...
            var __current_user_tokens =
                await this.GetTokensByUserIdAsync(apiUserToken.UserId);

            if (__current_user_tokens == null)
                return;

            // Get ClaimsIdentity...
            var __claimsIdentity = ((ClaimsIdentity)user.Identity);

            // Update Claims
            foreach (var __token in __current_user_tokens)
            {
                if (!__claimsIdentity.HasClaim(c => c.Type == __token.TokenType))
                    __claimsIdentity.AddClaim(new Claim(__token.TokenType, __token.Token));
            }
        }

        
    }
}
