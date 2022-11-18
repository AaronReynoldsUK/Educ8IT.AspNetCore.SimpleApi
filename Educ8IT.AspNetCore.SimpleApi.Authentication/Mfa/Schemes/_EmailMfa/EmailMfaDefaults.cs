using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailMfaDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SCHEME_NAME = "EmailMfa";

        /// <summary>
        /// 
        /// </summary>
        public const string SCHEME_DISPLAY_NAME = "MFA over Email";

        /// <summary>
        /// 
        /// </summary>
        public const SimpleApi.Identity.EMfaMethod METHOD = SimpleApi.Identity.EMfaMethod.Email;
    }
}
