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
    public class BearerAuthenticationOptions : AuthenticationSchemeOptions //, IHasEAuthenticationType
    {
        /// <summary>
        /// 
        /// </summary>
        public BearerAuthenticationOptions() { }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ExcludeFromAuthentication { get; set; } = new List<string>()
        {
            "/favicon.ico",
            "/robots.txt"
        };

        /// <summary>
        /// 
        /// </summary>
        public List<string> ClaimsHandled { get; set; } = new List<string>()
        {
            AuthenticationClaimTypes.Authenticated
        };

        /// <summary>
        /// 
        /// </summary>
        public PathString LoginPath { get; set; } = BearerAuthenticationDefaults.LoginPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString RefreshPath { get; set; } = BearerAuthenticationDefaults.RefreshPath;

        /// <summary>
        /// 
        /// </summary>
        public PathString LogoutPath { get; set; } = BearerAuthenticationDefaults.LogoutPath;
        
        /// <summary>
        /// 
        /// </summary>
        public string AccessTokenName { get; set; } = BearerAuthenticationDefaults.AccessTokenName;

        /// <summary>
        /// TTL for an Access Token.
        /// </summary>
        public int AccessTokenTTL { get; set; } = BearerAuthenticationDefaults.AccessTokenTTL;

        /// <summary>
        /// 
        /// </summary>
        public string RefreshTokenName { get; set; } = BearerAuthenticationDefaults.RefreshTokenName;
        
        /// <summary>
        /// TTL for a Refresh Token.
        /// </summary>
        public int RefreshTokenTTL { get; set; } = BearerAuthenticationDefaults.RefreshTokenTTL;

        ///// <summary>
        ///// 
        ///// </summary>
        //public new BearerAuthenticationEvents Events
        //{
        //    get => (BearerAuthenticationEvents)base.Events!;
        //    set => base.Events = value;
        //}

        //#region IHasEAuthenticationType

        ///// <summary>
        ///// 
        ///// </summary>
        //public EAuthenticationType AuthenticationType { get; set; } = EAuthenticationType.Identity;

        //#endregion
    }
}
