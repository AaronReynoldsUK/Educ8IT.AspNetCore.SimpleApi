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
        /// 
        /// </summary>
        public static readonly PathString LoginPath = new PathString("/auth/bearer/login");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString RefreshPath = new PathString("/auth/bearer/refresh");

        /// <summary>
        /// 
        /// </summary>
        public static readonly PathString LogoutPath = new PathString("/auth/bearer/logout");

        /// <summary>
        /// 
        /// </summary>
        public const string RequestGrantName = "bearer.access.request";

        /// <summary>
        /// 
        /// </summary>
        public const string AccessTokenName = "bearer.access";

        /// <summary>
        /// Default TTL for an Access Token.
        /// Default is 3600s = 1 hour
        /// </summary>
        public const int AccessTokenTTL = 3600;

        /// <summary>
        /// 
        /// </summary>
        public const string RefreshGrantName = "bearer.access.refresh";

        /// <summary>
        /// 
        /// </summary>
        public const string RefreshTokenName = "bearer.refresh";

        /// <summary>
        /// Default TTL for a Refresh Token.
        /// Default is 86400s = 1 day
        /// </summary>
        public const int RefreshTokenTTL = 86400;

        /// <summary>
        /// 
        /// </summary>
        public const string ExtendedDataKey_RemoteIp = "RemoteIp";

        /// <summary>
        /// 
        /// </summary>
        public const string RemoveGrantName = "bearer.access.remove";
    }
}
