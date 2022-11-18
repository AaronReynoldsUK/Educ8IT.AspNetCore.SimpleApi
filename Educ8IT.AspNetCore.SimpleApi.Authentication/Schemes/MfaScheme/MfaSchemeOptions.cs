using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> ClaimsHandled { get; set; } = new List<string>()
        {
            AuthenticationClaimTypes.Mfa
        };

        /// <summary>
        /// 
        /// </summary>
        public PathString RequestPath { get; set; } = MfaSchemeDefaults.RequestPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString VerifyPath { get; set; } = MfaSchemeDefaults.VerifyPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString RemovePath { get; set; } = MfaSchemeDefaults.RemovePath;

        /// <summary>
        /// 
        /// </summary>
        public string RequestTokenName { get; set; } = MfaSchemeDefaults.RequestTokenName;

        /// <summary>
        /// TTL for an Access Token.
        /// </summary>
        public int RequestTokenTTL { get; set; } = MfaSchemeDefaults.RequestTokenTTL;

        /// <summary>
        /// 
        /// </summary>
        public string VerifyTokenName { get; set; } = MfaSchemeDefaults.VerifyTokenName;


    }
}
