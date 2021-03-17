// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class BearerAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public BearerAuthenticationOptions()
        {
            TokenTTL = BearerAuthenticationDefaults.TokenTTL;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ExcludeFromAuthentication { get; set; } = new List<string>()
        {
            "/favicon.ico"
        };

        /// <summary>
        /// TTL for a Bearer Token.
        /// Default is 86400s = 1 day
        /// </summary>
        public int TokenTTL { get; set; } = BearerAuthenticationDefaults.TokenTTL;

        /// <summary>
        /// 
        /// </summary>
        public PathString LoginPath = BearerAuthenticationDefaults.LoginPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString LogoutPath = BearerAuthenticationDefaults.LogoutPath;

        /// <summary>
        /// 
        /// </summary>
        public new BearerAuthenticationEvents Events
        {
            get => (BearerAuthenticationEvents)base.Events!;
            set => base.Events = value;
        }
    }
}
