using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// Used in Multi-Stage Authentication
    /// </summary>
    [Flags]
    public enum EAuthenticationType
    {
        /// <summary>
        /// Not Authenticated
        /// </summary>
        None = 0,

        /// <summary>
        /// User/Identity is Authenticated
        /// </summary>
        Identity = 1,

        /// <summary>
        /// Email Address is Confirmed
        /// </summary>
        Email = 2,

        /// <summary>
        /// MFA has been performed
        /// </summary>
        MFA = 4
    }
}
