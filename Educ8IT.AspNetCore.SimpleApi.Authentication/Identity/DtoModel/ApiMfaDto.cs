using System.ComponentModel.DataAnnotations;
using System;
using Educ8IT.AspNetCore.SimpleApi.Dto;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Identity.DtoModel
{
    /// <summary>
    /// DTO for GET operations
    /// </summary>
    [Serializable]
    public class ApiMfa_ExportDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiMfa_ExportDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public ApiMfa_ExportDto(SimpleApi.Identity.ApiMfa entity)
        {
            this.Id = entity.Id;

            this.FriendlyName = entity.FriendlyName;
            this.Method = entity.Method;
            this.Parameters = entity.Parameters;
            this.PublicInfo = entity.PublicInfo;
            this.UserId = entity.UserId;
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
        [MaxLength(100)]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleApi.Identity.EMfaMethod Method { get; set; }

        /// <summary>
        /// Configuration Url
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string PublicInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        #endregion
    }

    /// <summary>
    /// DTO for POST operations
    /// </summary>
    [Serializable]
    public class ApiMfa_InsertDto : ValidationBaseDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiMfa_InsertDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void MapToDbModel(SimpleApi.Identity.ApiMfa entity)
        {
            entity.FriendlyName = this.FriendlyName;
            entity.Method = this.Method;
            entity.Parameters = this.Parameters;
            entity.PublicInfo = this.PublicInfo;
            entity.UserId = this.UserId;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleApi.Identity.EMfaMethod Method { get; set; }

        /// <summary>
        /// Configuration Url
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string PublicInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        #endregion
    }

    /// <summary>
    /// DTO for PUT/PATCH operations
    /// </summary>
    [Serializable]
    public class ApiMfa_UpdateDto : ValidationBaseDto
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiMfa_UpdateDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void MapToDbModel(SimpleApi.Identity.ApiMfa entity)
        {
            entity.Id = this.Id;

            entity.FriendlyName = this.FriendlyName;
            entity.Method = this.Method;
            entity.Parameters = this.Parameters;
            entity.PublicInfo = this.PublicInfo;
            entity.UserId = this.UserId;
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
        [MaxLength(100)]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleApi.Identity.EMfaMethod Method { get; set; }

        /// <summary>
        /// Configuration Url
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string PublicInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        #endregion
    }
}
