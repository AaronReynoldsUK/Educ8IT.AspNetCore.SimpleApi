// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// Context object passed to the IBearerAuthenticationEvents method SignedIn. ?????
    /// </summary>    
    [Obsolete()]
    public class BearerSignedInContext : PrincipalContext<BearerAuthenticationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="principal"></param>
        /// <param name="properties"></param>
        /// <param name="options"></param>
        public BearerSignedInContext(
            HttpContext context,
            AuthenticationScheme scheme,
            ClaimsPrincipal principal,
            AuthenticationProperties? properties,
            BearerAuthenticationOptions options)
            : base (context, scheme, options, properties)
        {
            Principal = principal;
        }
    }
}
