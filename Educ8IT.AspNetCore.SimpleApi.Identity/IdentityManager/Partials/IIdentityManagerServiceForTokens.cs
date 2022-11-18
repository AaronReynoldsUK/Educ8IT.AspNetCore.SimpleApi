
namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public partial interface IIdentityManagerService
    {
        #region For Tokens

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ApiUserToken> GetTokenByTokenIdAsync(System.Guid tokenId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ApiUserToken> GetTokenByTokenValueAsync(string tokenValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ApiUserToken> GetTokenByTokenValueAsync(string tokenValue, string tokenType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ApiUserToken> GetTokenByTokenTypeForUserAsync(System.Guid userId, string tokenType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<System.Collections.Generic.List<ApiUserToken>> GetTokensByUserIdAsync(System.Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GenerateNewToken(int length = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUserToken"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task StoreTokenAsync(ApiUserToken apiUserToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task RemoveTokenAsync(string token, string tokenType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="transientOnly"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task RemoveUserTokensAsync(System.Guid userId, bool transientOnly, string tokenType = null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task RemoveExpiredTokensAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenType"></param>
        /// <param name="ttl"></param>
        /// <param name="extendedData"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<ApiUserToken> NewTokenAsync(
            System.Guid userId, string tokenType, int? ttl,
            System.Collections.Generic.Dictionary<string, object> extendedData);

        #endregion
    }
}
