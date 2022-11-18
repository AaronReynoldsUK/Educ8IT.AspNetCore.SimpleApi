// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Identity.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiMfa
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
        [MaxLength(100)]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EMfaMethod Method { get; set; }

        /// <summary>
        /// Configuration Url
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string PublicInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        #endregion

        #region Non-Mapped Parameters

        /// <summary>
        /// mfa://{method}/{label}:{account}?params
        /// NOT method specific like:
        /// e.g. "otpauth://type/label:account?qKey=qValue&qKey2=qValue2"
        /// </summary>
        public MfaDataUri GetConfigurationUri()
        {
            if (Parameters == null)
                return null;

            try
            {
                var __configurationUri = new MfaDataUri(Parameters);

                if (__configurationUri.IsValid)
                    return __configurationUri;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaDataUri"></param>
        public void SetConfigurationUri(MfaDataUri mfaDataUri)
        {
            if (mfaDataUri == null)
                return;

            if (!mfaDataUri.IsValid)
                return;

            Parameters = mfaDataUri.AbsoluteUri;
        }

        #endregion

        #region Db Model Link Properties

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApiUser User { get; set; }

        #endregion

        #region Validation

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public bool IsValid()
        //{
        //    bool __isValid = false;

        //    switch (Method)
        //    {
        //        case EMfaMethod.Email:
        //            __isValid = MailToUri.TryParse(this.Parameters, out MailToUri mailToUri);
        //            break;

        //        case EMfaMethod.SMS:
        //            __isValid = SmsUri.TryParse(this.Parameters, out SmsUri smsUri);
        //            break;

        //        case EMfaMethod.Telephone:
        //            __isValid = TelUri.TryParse(this.Parameters, out TelUri telUri);
        //            break;

        //        case EMfaMethod.TOTP:
        //            __isValid = OtpUri.TryParse(this.Parameters, out OtpUri otpUri);
        //            break;
        //    }

        //    return __isValid;
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbEntity"></param>
        public void UpdateFrom(ApiMfa dbEntity)
        {
            this.FriendlyName = dbEntity.FriendlyName;
            this.Method = dbEntity.Method;            
            this.Parameters = dbEntity.Parameters;
            this.PublicInfo = dbEntity.PublicInfo;
        }
    }
}
