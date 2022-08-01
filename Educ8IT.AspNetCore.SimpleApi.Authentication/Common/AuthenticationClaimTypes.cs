using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationClaimTypes
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RequiredClaim = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.RequiredClaim";

        /// <summary>
        /// Identity confirmed via user/password or similar authentication.
        /// </summary>
        public const string Authenticated = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.Authenticated";

        /// <summary>
        /// Identity Id from data source.
        /// </summary>
        public const string UserId = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.UserId";

        /// <summary>
        /// Identity UserName from data source.
        /// </summary>
        public const string UserName = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.UserName";

        /// <summary>
        /// Email Address confirmed
        /// </summary>
        public const string EmailConfirmed = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.EmailConfirmed";

        /// <summary>
        /// MFA completed
        /// </summary>
        public const string Mfa = "Educ8IT.AspNetCore.SimpleApi.Authentication.AuthenticationClaimTypes.Mfa";
    }
}
