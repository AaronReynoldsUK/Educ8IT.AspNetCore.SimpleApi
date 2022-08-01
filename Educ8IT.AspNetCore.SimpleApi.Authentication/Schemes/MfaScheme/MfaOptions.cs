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
    public class MfaOptions : AuthenticationSchemeOptions
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
        public PathString RequestPath { get; set; } = MfaDefaults.RequestPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString VerifyPath { get; set; } = MfaDefaults.VerifyPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString RemovePath { get; set; } = MfaDefaults.RemovePath;

        /// <summary>
        /// 
        /// </summary>
        public string RequestTokenName { get; set; } = MfaDefaults.RequestTokenName;

        /// <summary>
        /// TTL for an Access Token.
        /// </summary>
        public int RequestTokenTTL { get; set; } = MfaDefaults.RequestTokenTTL;

        /// <summary>
        /// 
        /// </summary>
        public string VerifyTokenName { get; set; } = MfaDefaults.VerifyTokenName;


    }
}
