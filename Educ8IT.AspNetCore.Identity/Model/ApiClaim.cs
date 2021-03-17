// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiClaim
    {
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
        [MaxLength(250)]
        public string ClaimType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ClaimValue { get; set; }
    }
}
