using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaSchemePostConfigureOptions : IPostConfigureOptions<MfaSchemeOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name, MfaSchemeOptions options)
        {

        }
    }
}
