// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthoriseAttribute : Attribute
    {
        /// <summary>
        /// Use default Authorisation policy
        /// </summary>
        public AuthoriseAttribute() { }

        /// <summary>
        /// Authorisation relies upon passing these Authorisation policies
        /// </summary>
        public string[] Policies { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<string> ImplicitAuthorisationPolicies
        //{
        //    get
        //    {
        //        var __listOfPolicies = new List<string>();

        //        if (RequiredRoleAuthorisation)
        //            __listOfPolicies.Add(DefaultAuthorisationPolicies.)
        //    }
        //}

        /// <summary>
        /// User must be a member of all these roles
        /// </summary>
        public string[] RequiredRoles { get; set; }

        /// <summary>
        /// User must be a member of at least one of these roles
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Indicates if any Role requirements have been added
        /// </summary>
        public bool RequiredRoleAuthorisation
        {
            get
            {
                return this.Roles?.Length > 0 || this.RequiredRoles?.Length > 0;
            }
        }

        ///// <summary>
        ///// Combine multiple Authorise Attributes
        ///// </summary>
        ///// <param name="authoriseAttribute"></param>
        //[Obsolete("Use AuthorisationExtensions instead")]
        //public void _CombineWith(AuthoriseAttribute authoriseAttribute)
        //{
        //    if (authoriseAttribute == null)
        //        return;

        //    this.Policies = Concatenation.Combine(this.Policies, authoriseAttribute.Policies, false);

        //    this.RequiredRoles = Concatenation.Combine(this.RequiredRoles, authoriseAttribute.RequiredRoles, false);

        //    this.Roles = Concatenation.Combine(this.Roles, authoriseAttribute.Roles, false);
        //}

        ///// <summary>
        ///// Combine an Authorize Attribute into this Authorise Attribute
        ///// </summary>
        ///// <param name="authorizeAttribute"></param>
        //[Obsolete("Use AuthorisationExtensions instead")]
        //public void _CombineWith(AuthorizeAttribute authorizeAttribute)
        //{
        //    if (authorizeAttribute == null)
        //        return;

        //    // Policy
        //    if (!String.IsNullOrEmpty(authorizeAttribute.Policy))
        //    {
        //        Policies.Add(authorizeAttribute.Policy);
        //    }

        //    // Roles
        //    if (!String.IsNullOrEmpty(authorizeAttribute.Roles))
        //    {
        //        var __roles = authorizeAttribute.Roles.Split(",", StringSplitOptions.RemoveEmptyEntries);
        //        if (__roles.Length == 1)
        //        {
        //            RequiredRoles.Add(__roles[0]);
        //        }
        //        else
        //        {
        //            __roles.ToList().ForEach(role =>
        //            {
        //                role = role.Trim();
        //                Roles.Add(role);
        //            });
        //        }
        //    }

        //    // Schemes... do we need this?
        //}
    }
}
