using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVerificationOptions : AuthenticationSchemeOptions //, IHasEAuthenticationType
    {
        //#region IHasEAuthenticationType

        ///// <summary>
        ///// 
        ///// </summary>
        //public EAuthenticationType AuthenticationType { get; set; } = EAuthenticationType.Email;

        //#endregion

        /// <summary>
        /// 
        /// </summary>
        public List<string> ClaimsHandled { get; set; } = new List<string>()
        {
            AuthenticationClaimTypes.EmailConfirmed
        };

        /// <summary>
        /// 
        /// </summary>
        public PathString RequestPath { get; set; } = EmailVerificationDefaults.RequestPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString VerifyPath { get; set; } = EmailVerificationDefaults.VerifyPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString RemovePath { get; set; } = EmailVerificationDefaults.RemovePath;

        /// <summary>
        /// 
        /// </summary>
        public string RequestTokenName { get; set; } = EmailVerificationDefaults.RequestTokenName;

        /// <summary>
        /// TTL for an Access Token.
        /// </summary>
        public int RequestTokenTTL { get; set; } = EmailVerificationDefaults.RequestTokenTTL;

        /// <summary>
        /// 
        /// </summary>
        public string VerifyTokenName { get; set; } = EmailVerificationDefaults.VerifyTokenName;

        /// <summary>
        /// 
        /// </summary>
        public string EmailSubject { get; set; } = EmailVerificationDefaults.EmailSubject;

        /// <summary>
        /// 
        /// </summary>
        public string EmailBody { get; set; } = EmailVerificationDefaults.EmailBody;

        /// <summary>
        /// 
        /// </summary>
        public bool EmailBodyIsHtml { get; set; } = EmailVerificationDefaults.EmailBodyIsHtml;
    }
}
