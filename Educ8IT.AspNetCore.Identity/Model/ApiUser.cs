// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiUser
    {
        #region Db Model Properties

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The Id in the external data set / context
        /// </summary>
        public Guid? LinkedId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EmailAddressIsConfirmed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccessFailedAttemptsTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccessFailedAttemptsCurrent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockoutUntil { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<ApiUserRoleLink> LinkedRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<ApiUserClaimLink> LinkedClaims { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<ApiUserToken> UserTokens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<ApiMfa> MfaOptions { get; set; }

        #endregion

        #region Fields

        private List<ApiClaim> _ApiClaims = null;
        private List<Claim> _Claims = null;
        private List<Claim> _RolesAsClaims = null;

        #endregion

        #region Non-Mapped Helper Properties

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool IsLockedOut
        {
            get { return this.LockoutUntil.HasValue && this.LockoutUntil.Value > DateTime.UtcNow; }
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool LockOutExpired
        {
            get { return this.LockoutUntil.HasValue && this.LockoutUntil.Value < DateTime.UtcNow; }
        }

        #endregion

        #region Non-Mapped Authorisation Properties

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public List<ApiClaim> ApiClaims
        {
            get
            {
                if (_ApiClaims == null)
                {
                    _ApiClaims = GetApiClaims();
                }
                return _ApiClaims;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public List<Claim> Claims
        {
            get
            {
                if (_Claims == null)
                {
                    _Claims = GetClaims();
                }
                return _Claims;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public List<Claim> RolesAsClaims
        {
            get
            {
                if (_RolesAsClaims == null)
                {
                    _RolesAsClaims = GetRolesAsClaims();
                }
                return _RolesAsClaims;
            }
        }

        #endregion

        #region Authorisation Helper Methods

        private List<ApiClaim> GetApiClaims()
        {
            var __listOfClaims = new List<ApiClaim>();
            if (this.LinkedClaims != null)
            {
                this.LinkedClaims.ToList().ForEach(linkedClaim =>
                {
                    __listOfClaims.Add(linkedClaim.Claim);
                });
            }
            if (this.LinkedRoles != null)
            {
                this.LinkedRoles.ToList().ForEach(linkedRole =>
                {
                    linkedRole.Role.LinkedClaims.ToList().ForEach(linkedClaim =>
                    {
                        __listOfClaims.Add(linkedClaim.Claim);
                    });
                });
            }
            return __listOfClaims;
        }

        private List<Claim> GetClaims()
        {
            var __listOfClaims = new List<Claim>();
            if (ApiClaims == null)
                return __listOfClaims;

            foreach (var __apiClaim in ApiClaims)
            {
                __listOfClaims.Add(new Claim(__apiClaim.ClaimType, __apiClaim.ClaimValue));
            }
            return __listOfClaims;
        }

        private List<Claim> GetRolesAsClaims()
        {
            var __listOfRoles = new List<Claim>();
            if (LinkedRoles != null)
            {
                foreach (var __role in LinkedRoles)
                {
                    __listOfRoles.Add(new Claim(ClaimTypes.Role, __role.Role.RoleName));
                }
            }
            return __listOfRoles;
        }

        #endregion
    }
}
