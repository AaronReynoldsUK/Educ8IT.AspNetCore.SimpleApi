// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation.Attributes
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
        public List<string> Policies { get; set; } = new List<string>();

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
        public List<string> RequiredRoles { get; set; } = new List<string>();

        /// <summary>
        /// User must be a member of at least one of these roles
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Indicates if any Role requirements have been added
        /// </summary>
        public bool RequiredRoleAuthorisation
        {
            get
            {
                return this.Roles?.Count > 0 || this.RequiredRoles?.Count > 0;
            }
        }

        /// <summary>
        /// Combine multiple Authorise Attributes
        /// </summary>
        /// <param name="authoriseAttribute"></param>
        public void CombineWith(AuthoriseAttribute authoriseAttribute)
        {
            if (authoriseAttribute == null)
                return;

            if ((authoriseAttribute.Policies?.Count ?? 0) > 0)
                this.Policies.AddRange(authoriseAttribute.Policies);

            if ((authoriseAttribute.RequiredRoles?.Count ?? 0) > 0)
                this.RequiredRoles.AddRange(authoriseAttribute.RequiredRoles);

            if ((authoriseAttribute.Roles?.Count ?? 0) > 0)
                this.RequiredRoles.AddRange(authoriseAttribute.Roles);
        }

        /// <summary>
        /// Combine an Authorize Attribute into this Authorise Attribute
        /// </summary>
        /// <param name="authorizeAttribute"></param>
        public void CombineWith(AuthorizeAttribute authorizeAttribute)
        {
            if (authorizeAttribute == null)
                return;

            // Policy
            if (!String.IsNullOrEmpty(authorizeAttribute.Policy))
            {
                Policies.Add(authorizeAttribute.Policy);
            }

            // Roles
            if (!String.IsNullOrEmpty(authorizeAttribute.Roles))
            {
                var __roles = authorizeAttribute.Roles.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (__roles.Length == 1)
                {
                    RequiredRoles.Add(__roles[0]);
                }
                else
                {
                    __roles.ToList().ForEach(role =>
                    {
                        role = role.Trim();
                        Roles.Add(role);
                    });
                }
            }

            // Schemes... do we need this?
        }
    }
}
