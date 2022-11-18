// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityManagerOptions : IIdentityManagerOptions
    {
        #region Authentication

        /// <summary>
        /// Limit the number of failed attempts to Authenticate.
        /// A value of 0 indicates no lock-outs to be enforced.
        /// </summary>
        public int LockoutAfterNFailedAttempts { get; set; } = 3;


        /// <summary>
        /// The period to lockout an account.
        /// The default is 900s = 15 minutes.
        /// </summary>
        public TimeSpan LockoutFor { get; set; } = TimeSpan.FromMinutes(15);

        #endregion

        #region Passwords

        /// <summary>
        /// Specify the length when generating new random passwords
        /// </summary>
        public int GeneratedPasswordLength { get; set; } = 32;

        /// <summary>
        /// Include lower-case letters when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeLowerCase { get; set; } = true;

        /// <summary>
        /// Include numbers when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeNumbers { get; set; } = true;

        /// <summary>
        /// Include symbols when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeSymbols { get; set; } = true;

        /// <summary>
        /// Include upper-case letters when generating new random passwords
        /// </summary>
        public bool GeneratedPasswordsIncludeUpperCase { get; set; } = true;

        #endregion

        #region Tokens

        /// <summary>
        /// Specify the length when generating new random tokens
        /// </summary>
        public int GeneratedTokenLength { get; set; } = 128;

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan AccessTokenLifespan { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan RefreshTokenLifespan { get; set; } = TimeSpan.FromHours(24);

        #endregion
    }
}
