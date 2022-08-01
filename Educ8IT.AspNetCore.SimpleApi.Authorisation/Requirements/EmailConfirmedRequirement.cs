// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailConfirmedRequirement: IAuthorisationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RequirementKey = "EmailConfirmed";

        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; set; } = RequirementKey;
    }
}
