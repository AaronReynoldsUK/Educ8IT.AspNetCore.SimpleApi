// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Models;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Dto
{
    /// <summary>
    /// Interface for Insert and Update DTOs (or POST / PUT)
    /// </summary>
    public interface IValidationBaseDto
    {
        #region IValidationBaseDto

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topLevelObject"></param>
        public abstract void Validate<T>(T topLevelObject) where T : ValidationBaseDto;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddValidationItem(string key, params string[] values);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddItem(string key, params string[] values);

        /// <summary>
        /// 
        /// </summary>
        public abstract ProblemDetails GetValidationProblemDetails();

        #endregion
    }
}
