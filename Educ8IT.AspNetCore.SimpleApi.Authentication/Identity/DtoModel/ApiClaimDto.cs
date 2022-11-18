using Educ8IT.AspNetCore.SimpleApi.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Identity.DtoModel
{
    /// <summary>
    /// DTO for GET operations
    /// </summary>
    [Serializable]
    public class ApiClaim_ExportDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiClaim_ExportDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public ApiClaim_ExportDto(SimpleApi.Identity.ApiClaim entity)
        {
            this.Id = entity.Id;
            this.ClaimType = entity.ClaimType;
            this.ClaimValue = entity.ClaimValue;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Required]
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

        #endregion
    }

    /// <summary>
    /// DTO for POST operations
    /// </summary>
    [Serializable]
    public class ApiClaim_InsertDto : ValidationBaseDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiClaim_InsertDto() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void MapToDbModel(SimpleApi.Identity.ApiClaim entity)
        {
            entity.ClaimType = this.ClaimType;
            entity.ClaimValue = this.ClaimValue;
        }

        #region Properties

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

        #endregion
    }

    /// <summary>
    /// DTO for PUT/PATCH operations
    /// </summary>
    [Serializable]
    public class ApiClaim_UpdateDto : ValidationBaseDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiClaim_UpdateDto() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void MapToDbModel(SimpleApi.Identity.ApiClaim entity)
        {
            entity.Id = this.Id;

            entity.ClaimType = this.ClaimType;
            entity.ClaimValue = this.ClaimValue;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Required]
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

        #endregion
    }
}
