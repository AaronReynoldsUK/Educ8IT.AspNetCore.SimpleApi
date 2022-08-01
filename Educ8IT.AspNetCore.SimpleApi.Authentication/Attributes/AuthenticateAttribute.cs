using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthenticateAttribute : Attribute
    {
        /// <summary>
        /// Use default Authentication policy
        /// </summary>
        public AuthenticateAttribute() { }

        /// <summary>
        /// Set AuthenticationType required
        /// </summary>
        /// <param name="authenticationTypeRequired"></param>
        public AuthenticateAttribute(EAuthenticationType authenticationTypeRequired)
            : this()
        {
            AuthenticationTypeRequired = authenticationTypeRequired;
        }

        /// <summary>
        /// Set AuthenticationType required
        /// </summary>
        /// <param name="claimsRequired"></param>
        public AuthenticateAttribute(string[] claimsRequired)
            : this()
        {
            ClaimsRequired = claimsRequired;
        }


        private EAuthenticationType _AuthenticationTypeRequired = EAuthenticationType.None;

        /// <summary>
        /// 
        /// </summary>
        public EAuthenticationType AuthenticationTypeRequired
        {
            get { return _AuthenticationTypeRequired; }
            set
            {
                List<string> tmp = new List<string>();

                if (value.HasFlag(EAuthenticationType.Identity))
                    tmp.AddIfNotExist(AuthenticationClaimTypes.Authenticated);

                if (value.HasFlag(EAuthenticationType.Email))
                {
                    tmp.AddIfNotExist(AuthenticationClaimTypes.Authenticated);
                    tmp.AddIfNotExist(AuthenticationClaimTypes.EmailConfirmed);

                    // Enforce Identity for EmailConfirmation
                    if (!value.HasFlag(EAuthenticationType.Identity))
                    {
                        value |= EAuthenticationType.Identity;
                    }
                }

                if (value.HasFlag(EAuthenticationType.MFA))
                {
                    tmp.AddIfNotExist(AuthenticationClaimTypes.Authenticated);
                    tmp.AddIfNotExist(AuthenticationClaimTypes.Mfa);

                    // Enforce Identity for MFA
                    if (!value.HasFlag(EAuthenticationType.Identity))
                    {
                        value |= EAuthenticationType.Identity;
                    }
                }

                _ClaimsRequired = tmp;
                _AuthenticationTypeRequired = value;
            }
        }


        private List<string> _ClaimsRequired = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public string[] ClaimsRequired
        {
            get { return _ClaimsRequired?.ToArray(); }
            set
            {
                EAuthenticationType tmp = EAuthenticationType.None;

                if (value == null || value.Length == 0)
                    return;

                var tmpList = new List<string>(value);

                if (tmpList.Contains(AuthenticationClaimTypes.Authenticated))
                    tmp |= EAuthenticationType.Identity;

                if (tmpList.Contains(AuthenticationClaimTypes.EmailConfirmed))
                {
                    tmp |= EAuthenticationType.Email;

                    // Enforce Identity for EmailConfirmation
                    if (!tmp.HasFlag(EAuthenticationType.Identity))
                    {
                        tmp |= EAuthenticationType.Identity;
                    }
                }   

                if (tmpList.Contains(AuthenticationClaimTypes.Mfa))
                {
                    tmp |= EAuthenticationType.MFA;

                    // Enforce Identity for MFA
                    if (!tmp.HasFlag(EAuthenticationType.Identity))
                    {
                        tmp |= EAuthenticationType.Identity;
                    }
                }   

                _AuthenticationTypeRequired = tmp;
                _ClaimsRequired = tmpList;
            }
        }
    }
}
