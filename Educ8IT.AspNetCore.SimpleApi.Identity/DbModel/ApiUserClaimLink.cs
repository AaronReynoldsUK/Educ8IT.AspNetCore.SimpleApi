// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiUserClaimLink
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApiUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        [Column(Order = 2)]
        public Guid ClaimId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("ClaimId")]
        public virtual ApiClaim Claim { get; set; }
    }
}
