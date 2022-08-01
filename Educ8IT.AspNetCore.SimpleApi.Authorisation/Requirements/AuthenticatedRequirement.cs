﻿// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticatedRequirement : IAuthorisationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; set; }
    }
}
