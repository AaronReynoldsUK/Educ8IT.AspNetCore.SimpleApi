using Educ8IT.AspNetCore.SimpleApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICanAuthenticateRequest : IAuthenticationHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CanAuthenticateRequest(HttpContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrinciple"></param>
        /// <param name="apiUser"></param>
        /// <param name="apiUserTokens"></param>
        /// <returns></returns>
        Task UpdateClaimsAsync(ClaimsPrincipal claimsPrinciple, ApiUser apiUser, List<ApiUserToken> apiUserTokens);
    }
}
