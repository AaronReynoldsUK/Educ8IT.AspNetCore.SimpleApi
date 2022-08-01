using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVerificationDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AuthorisationHeaderName = "X-Authentication-EVS";

        /// <summary>
        /// 
        /// </summary>
        public const string AuthenticationScheme = "EmailVerification";

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString RequestPath = new PathString("/auth/email/request");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString VerifyPath = new PathString("/auth/email/verify");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString RemovePath = new PathString("/auth/email/remove");

        /// <summary>
        /// 
        /// </summary>
        public const string RequestGrantName = "email.verification.request";

        /// <summary>
        /// 
        /// </summary>
        public const string RequestTokenName = "email.verification.request";

        /// <summary>
        /// 
        /// </summary>
        public const int RequestTokenTTL = 3600;

        /// <summary>
        /// 
        /// </summary>
        public const string VerifyGrantName = "email.verification.verify";

        /// <summary>
        /// 
        /// </summary>
        public const string VerifyTokenName = "email.verification.verify";

        /// <summary>
        /// 
        /// </summary>
        public const string RemoveGrantName = "email.verification.remove";

        /// <summary>
        /// 
        /// </summary>
        public const string EmailSubject = "Email Address Confirmation";

        /// <summary>
        /// 
        /// </summary>
        public const string EmailBody = "Confirmation code for email address $$EMAIL_ADDRESS$$ is $$CONFIRMATION_CODE$$";

        /// <summary>
        /// 
        /// </summary>
        public const bool EmailBodyIsHtml = false;
    }
}
