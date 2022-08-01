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
    /// Context object passed to the <see cref="BearerAuthenticationEvents.SigningIn(BearerSigningInContext)"/>.
    /// </summary>
    [Obsolete()]
    public class BearerSigningInContext : PrincipalContext<BearerAuthenticationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="principal"></param>
        /// <param name="properties"></param>
        public BearerSigningInContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BearerAuthenticationOptions options,
            ClaimsPrincipal principal,
            AuthenticationProperties? properties)
            : base (context, scheme, options, properties)
        {
            Principal = principal;
        }
    }
}
