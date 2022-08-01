// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Newtonsoft.Json;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiUserToken
    {
        #region Db Model Properties

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApiUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// e.g. auth_token, mfa_token, refresh_token, email_confirmation_token
        /// </summary>
        [Required]
        public string TokenType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExtendedDataInDb
        {
            get
            {
                return (ExtendedData != null && ExtendedData.Count > 0)
                    ? JsonConvert.SerializeObject(ExtendedData)
                    : null;
            }
            set
            {
                try
                {
                    ExtendedData = (value != null)
                        ? JsonConvert.DeserializeObject<Dictionary<string, object>>(value)
                        : new Dictionary<string, object>();
                }
                catch { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public Dictionary<string, object> ExtendedData { get; private set; } = new Dictionary<string, object>();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool IsExpired
        {
            get
            {
                return ValidUntil.HasValue && ValidUntil < DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extendedData"></param>
        public void SetExtendedData(Dictionary<string, object> extendedData)
        {
            this.ExtendedData = extendedData;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool HasExtendedData
        {
            get
            {
                return this.ExtendedData != null;
            }
        }
    }
}
