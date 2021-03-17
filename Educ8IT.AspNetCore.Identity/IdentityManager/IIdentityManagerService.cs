// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIdentityManagerService
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasDbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiUser> GetUserByNameAsync(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> IsValidPasswordAsync(ApiUser apiUser, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <returns></returns>
        Task<bool> CanSignIn(ApiUser apiUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <returns></returns>
        Task RecordFailedAuthentication(ApiUser apiUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <returns></returns>
        Task ResetFailedAuthentication(ApiUser apiUser);

        /// <summary>
        /// Generates a new random password using the Options specified in the setup action on IdentityManager
        /// </summary>
        /// <returns></returns>
        Task<string> GenerateNewPassword();

        /// <summary>
        /// This will generate a new password hash for the supplied password. 
        /// Use GenerateNewPassword() to get a new random password.
        /// The hashed password is updated on the ApiUser object and saved to the data store.
        /// The hash password is returned.
        /// </summary>
        /// <param name="apiUser">The ApiUser which will be updated with the new hashed password</param>
        /// <param name="password">The password to HASH.</param>
        /// <returns></returns>
        Task UpdatePasswordHash(ApiUser apiUser, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUserToken"></param>
        /// <returns></returns>
        Task StoreToken(ApiUserToken apiUserToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        Task RemoveToken(string token, string tokenType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RemoveExpiredTokens();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">The authentication token provided</param>
        /// <param name="scheme">e.g. Bearer</param>
        /// <param name="refreshUsingTTL">If supplied, the service will extend the token lifespan using this provided value</param>
        /// <returns></returns>
        Task<ApiUser> AuthenticateTokenAsync(string token, string scheme, int? refreshUsingTTL = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        Task<ApiUser> RegisterAsync(string emailAddress, string password, string displayName = null);

        //public Task<ApiUser> AuthenticateTokenAsync(string token)
        //{
        //    ApiUser __authUser = new ApiUser();

        //    return Task.FromResult(__authUser);
        //}

        //public ApiUser Authenticate (string userName, string password)
        //{
        //    return new ApiUser();
        //}

        //public void SignInUser(ApiUser apiUser, string authenticationType)
        //{
        //    var __claims = new List<Claim>();
        //    //__claims.Add(new Claim());
        //    var __identity = new ClaimsIdentity(__claims, BearerAuthenticationDefaults.AuthenticationScheme);
        //    var __principle = new ClaimsPrincipal(__identity);

        //    var __ticket = new AuthenticationTicket(__principle,
        //        new AuthenticationProperties()
        //        {

        //        })
        //}
    }
}
