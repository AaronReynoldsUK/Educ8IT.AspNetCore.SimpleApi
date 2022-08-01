using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorisationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        string SchemeName { get; set; }
    }
}
