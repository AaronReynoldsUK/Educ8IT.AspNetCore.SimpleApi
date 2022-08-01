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
    /// 
    /// </summary>
    [Obsolete()]
    public class BearerValidatePrincipleContext : PrincipalContext<BearerAuthenticationOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="ticket"></param>
        public BearerValidatePrincipleContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BearerAuthenticationOptions options,
            AuthenticationTicket ticket)
            : base(context, scheme, options, ticket.Properties)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            Principal = ticket.Principal;
        }

        //public SecurityToken SecurityToken { get; set; }

        /// <summary>
        /// Called to replace the claims principal. The supplied principal will replace the value of the 
        /// Principal property, which determines the identity of the authenticated request.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> used as the replacement</param>
        public void ReplacePrincipal(ClaimsPrincipal principal) => Principal = principal;

        /// <summary>
        /// Called to reject the incoming principal. This may be done if the application has determined the
        /// account is no longer active, and the request should be treated as if it was anonymous.
        /// </summary>
        public void RejectPrincipal() => Principal = null;
    }
}
