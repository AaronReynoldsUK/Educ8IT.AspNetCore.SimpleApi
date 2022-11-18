// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.Dto;
using Educ8IT.AspNetCore.SimpleApi.Models;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    /// <summary>
    /// Authentication request
    /// </summary>
    [Serializable]
    [JsonObject(Description = "Used to Authenticate with the API")]
    public class BearerTokenRequest : ValidationBaseDto
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public static string[] ALLOWED_GRANT_TYPES = new string[]
        {
            "password",
            "refresh_token",

            "bearer.access.request",
            "bearer.access.refresh",
            "bearer.access.remove",

            "email.verification.request",
            "email.verification.verify",
            "email.verification.remove",

            "mfa.verification.request",
            "mfa.verification.verify",
            "mfa.verification.remove"
        };

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BearerTokenRequest() : base() { }

        #region Shared Properties

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("grant_type")]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("username")]
        public string UserName { get; set; }

        #endregion

        #region Properties for GrantType = "password" | "bearer.access.request"

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("password")]
        public string Password { get; set; }

        #endregion

        #region Properties for GrantType = "refresh_token" | "bearer.access.refresh"

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("refresh_token")]
        public string RefreshToken { get; set; }

        #endregion

        #region Properties for GrantType = "bearer.access.remove"

        #endregion

        #region Properties for GrantType = "email.verification.verify"

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("confirmation_token")]
        public string ConfirmationToken { get; set; }

        #endregion

        #region Properties for GrantType = "mfa.verification.request" | "mfa.verification.verify" | "mfa.verification.remove"

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("mfa_method_id")]
        public string MfaMethodId { get; set; }

        #endregion

        #region Properties for GrantType = "mfa.verification.verify"

        /// <summary>
        /// 
        /// </summary>
        [PropertyAlias("mfa_code")]
        public string MfaCode { get; set; }

        #endregion

        #region Validation

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topLevelObject"></param>
        public override void Validate<T>(T topLevelObject)
        {
            base.Validate(topLevelObject);

            if (!GrantType.IsValid(ALLOWED_GRANT_TYPES, false, false))
            {
                topLevelObject.AddValidationItem(nameof(GrantType), $"Missing or Invalid");
            }

            switch (GrantType)
            {
                case "password":
                case "bearer.access.request":
                    ValidateFor_Password(topLevelObject);
                    break;
                case "refresh_token":
                case "bearer.access.refresh":
                    ValidateFor_RefreshToken(topLevelObject);
                    break;


                case "email.verification.request":
                    ValidateFor_EmailConfirmationRequest(topLevelObject);
                    break;
                case "email.verification.verify":
                    ValidateFor_EmailConfirmationVerify(topLevelObject);
                    break;
                case "email.verification.remove":
                    ValidateFor_EmailConfirmationRemove(topLevelObject);
                    break;

                case "mfa.verification.request":
                    ValidateFor_MfaRequest(topLevelObject);
                    break;
                case "mfa.verification.verify":
                    ValidateFor_MfaVerify(topLevelObject);
                    break;
                case "mfa.verification.remove":
                    ValidateFor_MfaRemove(topLevelObject);
                    break;
            }
        }

        private void ValidateFor_Password<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(Password) || !Password.IsValid(RegularExpressions.REGEX_PASSWORD, false))
                topLevelObject.AddValidationItem(nameof(Password), $"Missing or Invalid");
        }

        private void ValidateFor_RefreshToken<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(RefreshToken) || !RefreshToken.IsValid(RegularExpressions.REGEX_TOKEN, false))
                topLevelObject.AddValidationItem(nameof(RefreshToken), $"Missing or Invalid");
        }

        private void ValidateFor_EmailConfirmationRequest<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");
        }

        private void ValidateFor_EmailConfirmationVerify<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(ConfirmationToken) || !ConfirmationToken.IsValid(RegularExpressions.REGEX_TOTP, false))
                topLevelObject.AddValidationItem(nameof(ConfirmationToken), $"Missing or Invalid");
        }

        private void ValidateFor_EmailConfirmationRemove<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");
        }

        private void ValidateFor_MfaRequest<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(MfaMethodId) || !MfaMethodId.IsValid(RegularExpressions.REGEX_UUID, false))
                topLevelObject.AddValidationItem(nameof(MfaMethodId), $"Missing or Invalid");
        }

        private void ValidateFor_MfaVerify<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(MfaMethodId) || !MfaMethodId.IsValid(RegularExpressions.REGEX_UUID, false))
                topLevelObject.AddValidationItem(nameof(MfaMethodId), $"Missing or Invalid");

            if (String.IsNullOrEmpty(MfaCode) || !MfaCode.IsValid(RegularExpressions.REGEX_TOTP, false))
                topLevelObject.AddValidationItem(nameof(MfaCode), $"Missing or Invalid");
        }

        private void ValidateFor_MfaRemove<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (String.IsNullOrEmpty(UserName) || !UserName.IsValid(RegularExpressions.REGEX_USERNAME, false))
                topLevelObject.AddValidationItem(nameof(UserName), $"Missing or Invalid");

            if (String.IsNullOrEmpty(MfaMethodId) || !MfaMethodId.IsValid(RegularExpressions.REGEX_UUID, false))
                topLevelObject.AddValidationItem(nameof(MfaMethodId), $"Missing or Invalid");
        }

        #endregion
    }
}
