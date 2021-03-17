// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// Used in object conversion to string
    /// </summary>
    public enum EConversionFormat
    {
        /// <summary>
        /// Normal string
        /// </summary>
        STRING = 0,

        /// <summary>
        /// Date string
        /// </summary>
        DATE,

        /// <summary>
        /// DateTime string
        /// </summary>
        DATETIME,

        /// <summary>
        /// Time string
        /// </summary>
        TIME,

        /// <summary>
        /// Currency string
        /// </summary>
        CURRENCY
    }
}
