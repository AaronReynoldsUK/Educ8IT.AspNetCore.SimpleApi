// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme.Model
{
    /// <summary>
    /// Returned when initial authentication is successful. 
    /// Always returns JSON. 
    /// Refer to https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/ for further details.
    /// </summary>
    [JsonObject("Success response")]
    [Obsolete()]
    public class ApiTokenResponse_Success
    {
        /// <summary>
        /// The bearer token needed to access the full API
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Always returns "bearer"
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = "bearer";

        /// <summary>
        /// TTL in seconds for this token
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiredInS
        {
            get
            {
                return ExpiryDT > DateTime.UtcNow
                    ? Convert.ToInt32(ExpiryDT.Subtract(DateTime.UtcNow).TotalSeconds)
                    : 0;
            }
        }

        /// <summary>
        /// Issue date/time for this token
        /// </summary>
        [JsonIgnore(), XmlIgnore()]
        public DateTime ValidFromDT { get; set; } = default;

        /// <summary>
        /// Issue date/time for this token
        /// </summary>
        [JsonProperty(".issued")]
        public string Issued
        {
            get
            {
                return this.ValidFromDT.ToUniversalTime().ToString("r");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore(), XmlIgnore()]
        public DateTime ExpiryDT { get; set; } = default;

        /// <summary>
        /// Expiry date/time for this token
        /// </summary>
        [JsonProperty(".expires")]
        public string Expires
        {
            get
            {
                return this.ExpiryDT.ToUniversalTime().ToString("r");
            }
        }
    }
}
