using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHasEAuthenticationType
    {
        /// <summary>
        /// 
        /// </summary>
        EAuthenticationType AuthenticationType { get; set; }
    }
}
