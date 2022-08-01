// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// Context object passed to the <see cref="BearerAuthenticationEvents.SigningOut(BearerSigningOutContext)"/>
    /// </summary>
    [Obsolete()]
    public class BearerSigningOutContext : PropertiesContext<BearerAuthenticationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="properties"></param>
        public BearerSigningOutContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BearerAuthenticationOptions options,
            AuthenticationProperties? properties)
            : base(context, scheme, options, properties)
        { }
    }
}
