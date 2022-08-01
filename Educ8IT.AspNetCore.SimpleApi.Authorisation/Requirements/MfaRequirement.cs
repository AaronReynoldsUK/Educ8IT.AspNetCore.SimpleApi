using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// With this requirement, the user must have passed MFA
    /// </summary>
    public class MfaRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RequirementKey = "MfaPassed";
    }
}
