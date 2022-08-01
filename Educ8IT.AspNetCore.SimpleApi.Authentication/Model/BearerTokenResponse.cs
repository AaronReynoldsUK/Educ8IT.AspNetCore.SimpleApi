// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject("Success response")]
    public class BearerTokenResponse
    {
        #region Shared Properties

        /// <summary>
        /// { bearer | email_confirmation | mfa | password_reset }
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Issue date/time for this token
        /// </summary>
        [JsonIgnore(), XmlIgnore()]
        public DateTime StartDT { get; set; } = default;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore(), XmlIgnore()]
        public DateTime ExpiryDT { get; set; } = default;

        /// <summary>
        /// Issue date/time for this token
        /// </summary>
        [JsonProperty(".issued")]
        public string Issued
        {
            get
            {
                return StartDT.ToUniversalTime().ToString("r");
            }
        }

        /// <summary>
        /// Expiry date/time for this token
        /// </summary>
        [JsonProperty(".expires")]
        public string Expires
        {
            get
            {
                return ExpiryDT.ToUniversalTime().ToString("r");
            }
        }

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

        #endregion

        #region Properties for Bearer Scheme

        /// <summary>
        /// The bearer token needed to access the full API
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Only serialise the AccessToken if supplied
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAccessToken()
        {
            return !string.IsNullOrEmpty(AccessToken);
        }

        /// <summary>
        /// The bearer token needed to get a new Bearer token upon expiry
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Only serialise the AccessToken if supplied
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRefreshToken()
        {
            return !string.IsNullOrEmpty(RefreshToken);
        }

        #endregion

        #region Extended Properties

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("extended_properties")]
        public Dictionary<string, object> KeyValuePairs { get; set; }

        /// <summary>
        /// Only serialise the KeyValuePairs if supplied
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeKeyValuePairs()
        {
            return KeyValuePairs != null && KeyValuePairs.Count > 0;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static BearerTokenResponse GetAccessToken(
            ApiUserToken accessToken,
            ApiUserToken refreshToken)
        {
            return new BearerTokenResponse()
            {
                AccessToken = accessToken.Token,
                ExpiryDT = accessToken.ValidUntil ?? DateTime.UtcNow,
                RefreshToken = refreshToken.Token,
                StartDT = accessToken.ValidFrom,
                TokenType = accessToken.TokenType,

                KeyValuePairs = accessToken.ExtendedData
            };
        }
    }
}
