// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IIdentityManagerService
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasDbContext { get; }

        

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiUser> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ApiUser> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<ApiUser> GetUserByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> IsValidPasswordAsync(ApiUser apiUser, string password);

        /// <summary>
        /// Checks if the account is disabled or locked out in some way
        /// </summary>
        /// <param name="apiUser"></param>
        /// <returns></returns>
        Task<bool> CanSignInAsync(ApiUser apiUser);

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
        Task<string> GenerateNewPassword(int length = 0);

        

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

        

        

        

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="apiUser"></param>
        ///// <param name="refreshToken"></param>
        ///// <param name="tokenType"></param>
        ///// <returns></returns>
        //Task RefreshAccessToken(ApiUser apiUser, string refreshToken, string tokenType);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="token">The authentication token provided</param>
        ///// <param name="scheme">e.g. Bearer</param>
        ///// <param name="refreshUsingTTL">If supplied, the service will extend the token lifespan using this provided value</param>
        ///// <returns></returns>
        //Task<ApiUser> AuthenticateTokenAsync(string token, string scheme, int? refreshUsingTTL = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        Task<ApiUser> RegisterAsync(string emailAddress, string password, string displayName = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUser"></param>
        /// <param name="schemeName"></param>
        /// <param name="extraClaims"></param>
        /// <returns></returns>
        abstract ClaimsPrincipal GetClaimsPrinciple(ApiUser apiUser, string schemeName, List<Claim> extraClaims = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="apiUserToken"></param>
        /// <returns></returns>
        Task UpdateClaims(ClaimsPrincipal user, ApiUserToken apiUserToken);

    }
}
