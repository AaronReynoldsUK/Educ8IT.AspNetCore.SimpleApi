using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial class IdentityManagerService
    {
        #region For Tokens

        /// <inheritdoc/>
        public async Task<ApiUserToken> GetTokenByTokenIdAsync(Guid tokenId)
        {
            if (tokenId == null || tokenId == Guid.Empty)
                return null;

            return await _identityDbContext.ApiUserTokens
                .FirstOrDefaultAsync(u => u.Id == tokenId);
        }

        /// <inheritdoc/>
        public async Task<ApiUserToken> GetTokenByTokenValueAsync(string tokenValue)
        {
            if (String.IsNullOrEmpty(tokenValue))
                return null;

            if (tokenValue != null)
                tokenValue = tokenValue.ToUpperInvariant();

            return await _identityDbContext.ApiUserTokens
                .FirstOrDefaultAsync(u => u.Token == tokenValue);
        }

        /// <inheritdoc/>
        public async Task<ApiUserToken> GetTokenByTokenValueAsync(string tokenValue, string tokenType)
        {
            if (String.IsNullOrEmpty(tokenValue))
                return null;

            if (String.IsNullOrEmpty(tokenType))
                return null;

            if (tokenValue != null)
                tokenValue = tokenValue.ToUpperInvariant();

            return await _identityDbContext.ApiUserTokens
                .FirstOrDefaultAsync(u => u.Token == tokenValue && u.TokenType == tokenType);
        }

        /// <inheritdoc/>
        public async Task<ApiUserToken> GetTokenByTokenTypeForUserAsync(System.Guid userId, string tokenType)
        {
            if (userId == null || userId == Guid.Empty)
                return null;

            if (String.IsNullOrEmpty(tokenType))
                return null;

            var predicate = PredicateBuilder.New<ApiUserToken>(true);

            predicate = predicate.And(p => p.UserId == userId);

            predicate = predicate.And(p => p.TokenType == tokenType);

            // Get oldest
            return await _identityDbContext.ApiUserTokens
                .AsExpandable()
                .Where(predicate)
                .OrderBy(o => o.ValidFrom)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ApiUserToken>> GetTokensByUserIdAsync(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                return null;

            var predicate = PredicateBuilder.New<ApiUserToken>(true);

            predicate = predicate.And(p => p.UserId == userId);

            predicate = predicate.And(p => !p.ValidUntil.HasValue || p.ValidUntil > DateTime.UtcNow);

            return await _identityDbContext.ApiUserTokens
                .AsExpandable()
                .Where(predicate)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public Task<string> GenerateNewToken(int length = 0)
        {
            if (_options == null)
                throw new Exception("Unable to obtain Identity Manager options");

            if (length == 0)
                length = _options.GeneratedTokenLength;

            var tokenValue = Passwords.GeneratePassword(length,
                false,
                _options.GeneratedPasswordsIncludeNumbers,
                _options.GeneratedPasswordsIncludeSymbols,
                _options.GeneratedPasswordsIncludeUpperCase);

            if (tokenValue != null)
                tokenValue = tokenValue.ToUpperInvariant();

            var __b64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenValue));

            if (__b64 != null)
                __b64 = __b64.ToUpperInvariant();

            return Task.FromResult(__b64);
        }

        /// <inheritdoc/>
        public async Task StoreTokenAsync(ApiUserToken apiUserToken)
        {
            if (apiUserToken == null)
                return;

            if (apiUserToken.Token != null)
                apiUserToken.Token = apiUserToken.Token.ToUpperInvariant();

            await _identityDbContext.ApiUserTokens.AddAsync(apiUserToken);
            await _identityDbContext.SaveChangesAsync();

            // We may add TokenCacheing in a separate Singleton later
        }

        /// <inheritdoc/>
        public async Task RemoveTokenAsync(string tokenValue, string tokenType)
        {
            if (String.IsNullOrEmpty(tokenValue))
                return;

            if (String.IsNullOrEmpty(tokenType))
                return;

            if (tokenValue != null)
                tokenValue = tokenValue.ToUpperInvariant();

            var __matchedToken = await _identityDbContext.ApiUserTokens
                .FirstOrDefaultAsync(t => t.Token == tokenValue && t.TokenType == tokenType);

            if (__matchedToken != null)
            {
                _identityDbContext.ApiUserTokens.Remove(__matchedToken);
                await _identityDbContext.SaveChangesAsync();
            }

            return;
        }

        /// <inheritdoc/>
        public async Task RemoveUserTokensAsync(Guid userId, bool transientOnly, string tokenType = null)
        {
            if (userId == null || userId == Guid.Empty)
                return;

            var predicate = PredicateBuilder.New<ApiUserToken>(true);

            predicate = predicate.And(p => p.UserId == userId);

            if (transientOnly)
                predicate = predicate.And(p => p.ValidUntil.HasValue);

            if (!String.IsNullOrEmpty(tokenType))
                predicate = predicate.And(p => p.TokenType == tokenType);

            var __matchedTokens = await _identityDbContext.ApiUserTokens
                .AsExpandable()
                .Where(predicate)
                .ToListAsync();

            if (__matchedTokens != null)
            {
                _identityDbContext.ApiUserTokens.RemoveRange(__matchedTokens);
                await _identityDbContext.SaveChangesAsync();
            }

            return;
        }

        /// <inheritdoc/>
        public async Task RemoveExpiredTokensAsync()
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
        public async Task<ApiUserToken> NewTokenAsync(
            Guid userId, string tokenType, int? ttl,
            Dictionary<string, object> extendedData)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            if (userId == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(userId));

            if (String.IsNullOrEmpty(tokenType))
                throw new ArgumentNullException(nameof(tokenType));

            // Wipe all previous tokens of type
            await this.RemoveUserTokensAsync(userId, false, tokenType);

            var __newTokenB64 = await GenerateNewToken();

            var __newToken = new ApiUserToken()
            {
                Id = Guid.NewGuid(),
                Token = __newTokenB64,
                TokenType = tokenType,
                UserId = userId,
                ValidFrom = DateTime.UtcNow
            };

            if (ttl.HasValue)
            {
                __newToken.ValidUntil = DateTime.UtcNow.AddSeconds(ttl.Value);
            }

            __newToken.SetExtendedData(extendedData);

            await this.StoreTokenAsync(__newToken);

            return __newToken;
        }

        #endregion
    }
}
