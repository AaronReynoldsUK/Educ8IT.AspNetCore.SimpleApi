using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailVerificationPostConfigureOptions: IPostConfigureOptions<EmailVerificationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name, EmailVerificationOptions options)
        {

        }
    }
}
