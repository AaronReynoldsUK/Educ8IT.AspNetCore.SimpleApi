// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiUserToken
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
        /// 
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
    }
}
