using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaOptionDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbMfa"></param>
        public MfaOptionDto(Identity.ApiMfa dbMfa)
        {
            Id = dbMfa.Id;
            Method = dbMfa.Method;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Identity.EMfaMethod Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        #endregion
    }
}
