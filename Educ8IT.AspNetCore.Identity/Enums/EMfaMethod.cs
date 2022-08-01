// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public enum EMfaMethod
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// Time-based One-time Password Algorithm
        /// </summary>
        TOTP,

        /// <summary>
        /// 
        /// </summary>
        Email,

        /// <summary>
        /// 
        /// </summary>
        SMS
    }
}
