using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaSchemeDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AuthorisationHeaderName = "X-Authentication-MFA";

        /// <summary>
        /// 
        /// </summary>
        public const string AuthenticationScheme = "MFA";

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString RequestPath = new PathString("/auth/mfa/request");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString VerifyPath = new PathString("/auth/mfa/verify");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString RemovePath = new PathString("/auth/mfa/remove");

        /// <summary>
        /// 
        /// </summary>
        public const string RequestGrantName = "mfa.verification.request";

        /// <summary>
        /// 
        /// </summary>
        public const string RequestTokenName = "mfa.verification.request";

        /// <summary>
        /// 
        /// </summary>
        public const int RequestTokenTTL = 3600;

        /// <summary>
        /// 
        /// </summary>
        public const string VerifyGrantName = "mfa.verification.verify";

        /// <summary>
        /// 
        /// </summary>
        public const string VerifyTokenName = "mfa.verification.verify";

        /// <summary>
        /// 
        /// </summary>
        public const string RemoveGrantName = "mfa.verification.remove";
    }
}
