// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationAwareApiMethodItem : AuthenticationAwareApiMethodItem
    {
        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public AuthorisationAwareApiMethodItem() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="apiControllerItem"></param>
        public AuthorisationAwareApiMethodItem(MethodInfo methodInfo, IApiControllerItem apiControllerItem)
            : base(methodInfo, apiControllerItem)
        {
            ParseAuthorisationAttributes();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AuthoriseAttribute AuthoriseAttribute { get; private set; }

        #endregion

        private void ParseAuthorisationAttributes()
        {
            AuthoriseAttribute = null;

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "AllowAnonymousAttribute")
                {
                    AuthoriseAttribute = null;
                    break;
                }
                else if (__attribute.Key == "AuthorizeAttribute")
                {
                    if (AuthoriseAttribute == null)
                        AuthoriseAttribute = new AuthoriseAttribute();

                    AuthoriseAttribute
                            .CombineWith(__attribute.Value as AuthorizeAttribute);
                }
                else if (__attribute.Key == "AuthoriseAttribute")
                {
                    if (AuthoriseAttribute == null)
                        AuthoriseAttribute = __attribute.Value as AuthoriseAttribute;
                    else
                        AuthoriseAttribute.CombineWith(__attribute.Value as AuthoriseAttribute);
                }
            }
        }
    }
}
