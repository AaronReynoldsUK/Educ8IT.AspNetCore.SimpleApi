using Educ8IT.AspNetCore.SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidationBaseDto : IValidationBaseDto
    {
        #region IValidationBaseDto

        /// <summary>
        /// Has this DTO passed Validation?
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore()]
        public bool IsValid
        {
            get
            {
                if (problemDetails == null)
                    Validate(this);

                return ((problemDetails.Validation?.Count ?? 0) == 0);
            }
        }

        /// <summary>
        /// Filled with any errors or validation problems
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore()]
        private ProblemDetails problemDetails { get; set; }

        /// <summary>
        /// Validate the Entity data in the DTO (overridden in sub-classes).
        /// </summary>
        /// <typeparam name="T">An object of T</typeparam>
        /// <param name="topLevelObject">An insance of a sub-class</param>
        public virtual void Validate<T>(T topLevelObject) where T : ValidationBaseDto
        {
            if (problemDetails == null)
                problemDetails = new ProblemDetails();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddValidationItem(string key, params string[] values)
        {
            if (problemDetails.Validation == null)
                problemDetails.Validation = new List<ProblemDetailsExtension>();

            if (!problemDetails.Validation.Exists(item => item.Key == key))
            {
                problemDetails.Validation.Add(new ProblemDetailsExtension()
                {
                    Key = key,
                    Items = values.ToList()
                });
            }
            else
            {
                problemDetails.Validation.FirstOrDefault(k => k.Key == key)
                    .Items.AddRange(values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddItem(string key, params string[] values)
        {
            if (problemDetails.Extensions == null)
                problemDetails.Extensions = new List<ProblemDetailsExtension>();

            if (!problemDetails.Extensions.Exists(item => item.Key == key))
            {
                problemDetails.Extensions.Add(new ProblemDetailsExtension()
                {
                    Key = key,
                    Items = values.ToList()
                });
            }
            else
            {
                problemDetails.Extensions.FirstOrDefault(k => k.Key == key)
                    .Items.AddRange(values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ProblemDetails GetValidationProblemDetails()
        {
            if (IsValid || problemDetails == null)
                return null;

            problemDetails.Detail = "There are validation errors";
            problemDetails.StatusCode = System.Net.HttpStatusCode.BadRequest;
            problemDetails.Title = "Validation Errors";

            return problemDetails;
        }

        #endregion
    }
}
