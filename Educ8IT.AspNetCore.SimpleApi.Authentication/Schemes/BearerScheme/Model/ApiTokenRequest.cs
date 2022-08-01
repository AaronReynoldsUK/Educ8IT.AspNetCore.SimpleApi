// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Newtonsoft.Json;
using System;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme.Model
{
    /// <summary>
    /// OAuth authentication request
    /// </summary>
    [JsonObject(Description = "Used to Authenticate with the API")]
    [Obsolete()]
    public class ApiTokenRequest
    {
        /// <summary>
        /// Must be "password"
        /// </summary>
        [PropertyAlias("grant_type")]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// As supplied by AYOM
        /// </summary>
        [PropertyAlias("username")]
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// As supplied by AYOM
        /// </summary>
        [PropertyAlias("password")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
