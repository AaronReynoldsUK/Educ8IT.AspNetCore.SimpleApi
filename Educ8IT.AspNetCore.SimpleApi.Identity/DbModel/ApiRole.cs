// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRole
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
        [MaxLength(100)]
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ApiRoleClaimLink> LinkedClaims { get; set; }
    }
}
