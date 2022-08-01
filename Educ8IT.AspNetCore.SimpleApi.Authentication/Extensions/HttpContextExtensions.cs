using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AuthenticationHeaderValue GetAuthenticationHeader(this HttpContext context)
        {
            if (context == null)
                return null;

            if (!context.Request.Headers.ContainsKey(Microsoft.Net.Http.Headers.HeaderNames.Authorization))
                return null;

            if (!AuthenticationHeaderValue.TryParse(
                context.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization],
                out AuthenticationHeaderValue headerValue))
            {
                return null;
            }

            return headerValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="headerName"></param>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        public static bool CanAuthenticateRequest(this HttpContext context, string headerName, string schemeName)
        {
            if (context == null)
                return false;

            if (String.IsNullOrEmpty(headerName))
                return false;

            if (String.IsNullOrEmpty(schemeName))
                return false;

            if (!context.Request.Headers.ContainsKey(headerName))
                return false;

            var __header = context.Request.Headers[headerName];

            if (!AuthenticationHeaderValue.TryParse(__header, out AuthenticationHeaderValue headerValue))
                return false;

            return schemeName == headerValue.Scheme;
        }
    }
}
