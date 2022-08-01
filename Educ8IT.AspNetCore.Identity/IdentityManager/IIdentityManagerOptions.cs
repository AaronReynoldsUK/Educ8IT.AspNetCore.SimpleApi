// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// Options object for the IdentityManager
    /// </summary>
    public interface IIdentityManagerOptions
    {
        #region Authentication

        /// <summary>
        /// Limit the number of failed attempts to Authenticate.
        /// A value of 0 indicates no lock-outs to be enforced.
        /// </summary>
        public int LockoutAfterNFailedAttempts { get; set; }

        /// <summary>
        /// The period to lockout an account
        /// </summary>
        public TimeSpan LockoutFor { get; set; }

        #endregion

        #region Passwords

        /// <summary>
        /// Specify the length when generating new random passwords
        /// </summary>
        public int GeneratedPasswordLength { get; set; }

        /// <summary>
        /// Include lower-case letters when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeLowerCase { get; set; }

        /// <summary>
        /// Include numbers when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeNumbers { get; set; }

        /// <summary>
        /// Include symbols when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeSymbols { get; set; }

        /// <summary>
        /// Include upper-case letters when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeUpperCase { get; set; }

        #endregion

        #region Tokens

        /// <summary>
        /// Specify the length when generating new random tokens
        /// </summary>
        public int GeneratedTokenLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan AccessTokenLifespan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan RefreshTokenLifespan { get; set; }

        #endregion
    }
}
