// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme.Model
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject]
    [Serializable]
    public class ApiBearerToken
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiBearerToken() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="keyValuePairs"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public static ApiBearerToken NewToken(
            Guid userId, string userName, Dictionary<string, object> keyValuePairs, int ttl) 
        {
            return new ApiBearerToken()
            {
                BearerToken = Passwords.GeneratePassword(128, true, true, true, true).SerialiseToBase64(),
                ExpiresDT = DateTime.UtcNow + TimeSpan.FromSeconds(ttl),
                KeyValuePairs = keyValuePairs,
                UserId = userId,
                UserName = userName,
                ValidFromDT = DateTime.UtcNow
            };
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ValidFromDT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiresDT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> KeyValuePairs { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        public string BearerToken { get; internal set; } = null;

        #endregion
    }
}
