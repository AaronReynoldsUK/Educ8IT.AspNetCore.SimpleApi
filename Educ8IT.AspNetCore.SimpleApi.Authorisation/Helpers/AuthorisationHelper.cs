using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthorisationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authoriseAttribute"></param>
        /// <param name="secondAttribute"></param>
        public static void CombineWith(this AuthoriseAttribute authoriseAttribute, AuthoriseAttribute secondAttribute)
        {
            if (authoriseAttribute == null)
                return;

            if (secondAttribute == null)
                return;

            authoriseAttribute.Policies = Concatenation.Combine(authoriseAttribute.Policies, secondAttribute.Policies, false);

            authoriseAttribute.RequiredRoles = Concatenation.Combine(authoriseAttribute.RequiredRoles, secondAttribute.RequiredRoles, false);

            authoriseAttribute.Roles = Concatenation.Combine(authoriseAttribute.Roles, secondAttribute.Roles, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authoriseAttribute"></param>
        /// <param name="secondAttribute"></param>
        public static void CombineWith(this AuthoriseAttribute authoriseAttribute, Microsoft.AspNetCore.Authorization.AuthorizeAttribute secondAttribute)
        {
            if (authoriseAttribute == null)
                return;

            if (secondAttribute == null)
                return;

            authoriseAttribute.Policies = Concatenation.Combine(authoriseAttribute.Policies, secondAttribute.Policy, false);

            if (!String.IsNullOrEmpty(secondAttribute.Roles))
            {
                List<string> __tmpRoles = new List<string>();
                var __roles = secondAttribute.Roles.Split(",", StringSplitOptions.RemoveEmptyEntries);
                __roles.ToList().ForEach(role =>
                {
                    __tmpRoles.Add(role.Trim());
                });

                if (__roles.Length == 1)
                    authoriseAttribute.RequiredRoles = Concatenation.Combine(authoriseAttribute.RequiredRoles, __roles, false);
                else
                    authoriseAttribute.Roles = Concatenation.Combine(authoriseAttribute.Roles, __roles, false);
            }
        }
    }
}
