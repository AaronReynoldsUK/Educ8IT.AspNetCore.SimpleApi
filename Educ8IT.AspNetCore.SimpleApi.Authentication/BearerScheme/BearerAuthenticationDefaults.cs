// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public class BearerAuthenticationDefaults
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AuthorisationHeaderName = "Authorization";

        /// <summary>
        /// 
        /// </summary>
        public const string AuthenticationScheme = "Bearer";

        /// <summary>
        /// Default TTL for a Bearer Token.
        /// </summary>
        public const int TokenTTL = 86400;

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString LoginPath = new PathString("/auth/login");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString LogoutPath = new PathString("/auth/logout");

        //public static readonly PathString AccessDeniedPath = new PathString("/auth");
    }
}
