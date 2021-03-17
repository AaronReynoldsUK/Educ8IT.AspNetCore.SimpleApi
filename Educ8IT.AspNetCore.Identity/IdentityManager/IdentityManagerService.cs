// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityManagerService : IIdentityManagerService
    {
        private readonly ILogger<IdentityManagerService> _logger;
        private readonly IdentityManagerOptions _options;
        private readonly IdentityDbContext _identityDbContext;

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
            _logger = loggerFactory?.CreateLogger<IdentityManagerService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        }

        /// <inheritdoc/>
        public bool HasDbContext
        {
            get
            {
                return _identityDbContext != null;
            }
        }

        /// <inheritdoc/>
        public async Task<ApiUser> GetUserByNameAsync(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return null;

            userName = userName.ToLowerInvariant();

            return await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        /// <inheritdoc/>
        public async Task<bool> IsValidPasswordAsync(ApiUser apiUser, string password)
        {
            bool result = false;

            if (apiUser == null)
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
        public async Task<bool> CanSignIn(ApiUser apiUser)
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
        public Task<string> GenerateNewPassword()
        {
            if (_options == null)
                throw new Exception("Unable to obtain Identity Manager options");

            var __password = Passwords.GeneratePassword(_options.GeneratedPasswordLength,
                _options.GeneratedPasswordsIncludeLowerCase,
                _options.GeneratedPasswordsIncludeNumbers,
                _options.GeneratedPasswordsIncludeSymbols,
                _options.GeneratedPasswordsIncludeUpperCase);

            return Task.FromResult(__password);
        }

        /// <inheritdoc/>
        public async Task UpdatePasswordHash(ApiUser apiUser, string password)
        {
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

        /// <inheritdoc/>
        public async Task StoreToken(ApiUserToken apiUserToken)
        {
            if (apiUserToken == null)
                return;

            await _identityDbContext.ApiUserTokens.AddAsync(apiUserToken);
            await _identityDbContext.SaveChangesAsync();

            // We may add TokenCacheing in a separate Singleton later

            return;
        }

        /// <inheritdoc/>
        public async Task RemoveToken(string token, string tokenType)
        {
            var __matchedToken = await _identityDbContext.ApiUserTokens.FirstOrDefaultAsync(t => t.Token == token && t.TokenType == tokenType);
            if (__matchedToken != null)
            {
                _identityDbContext.ApiUserTokens.Remove(__matchedToken);
                await _identityDbContext.SaveChangesAsync();
            }

            return;
        }

        /// <inheritdoc/>
        public async Task RemoveExpiredTokens()
        {
            var __oldTokens = _identityDbContext.ApiUserTokens
                .Where(t => t.ValidUntil.HasValue && t.ValidUntil < DateTime.UtcNow);

            foreach (var __token in __oldTokens)
            {
                _identityDbContext.ApiUserTokens.Remove(__token);
            }

            await _identityDbContext.SaveChangesAsync();

            return;
        }

        /// <inheritdoc/>
        public async Task<ApiUser> AuthenticateTokenAsync(string token, string scheme, int? refreshUsingTTL = null)
        {
            if (String.IsNullOrEmpty(token))
                return null;

            var __tokenRecord = await _identityDbContext.ApiUserTokens.FirstOrDefaultAsync(t => t.Token == token && t.TokenType == scheme);

            if (__tokenRecord == null)
                return null;

            if (refreshUsingTTL.HasValue && refreshUsingTTL.Value > 0)
            {
                __tokenRecord.ValidUntil = DateTime.UtcNow.AddSeconds(refreshUsingTTL.Value);
                _identityDbContext.ApiUserTokens.Update(__tokenRecord);
                await _identityDbContext.SaveChangesAsync();
            }            

            var __authUser = await _identityDbContext.ApiUsers.FirstOrDefaultAsync(u => u.Id == __tokenRecord.UserId);
            if (__authUser.LinkedClaims != null)
            {
                foreach (var __linkedClaim in __authUser.LinkedClaims)
                {
                    var __claim = __linkedClaim.Claim;
                }
            }
            if (__authUser.LinkedRoles != null)
            {
                foreach (var __linkedClaim in __authUser.LinkedRoles)
                {
                    var __role = __linkedClaim.Role;
                    if (__role.LinkedClaims != null)
                    { }
                }
            }
            return __authUser;
        }

        /// <inheritdoc/>
        public async Task<ApiUser> RegisterAsync(string emailAddress, string password, string displayName = null)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return null;

            if (String.IsNullOrEmpty(password))
                return null;

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
    }
}
