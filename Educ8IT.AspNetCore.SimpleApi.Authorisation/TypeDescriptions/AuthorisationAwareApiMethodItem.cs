// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation.Attributes;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationAwareApiMethodItem : ApiMethodItem
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

        private void ParseAuthorisationAttributes()
        {
            AuthoriseAttribute = null;

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "AllowAnonymousAttribute")
                {
                    this.AuthoriseAttribute = null;
                    break;
                }
                else if (__attribute.Key == "AuthorizeAttribute")
                {
                    if (this.AuthoriseAttribute == null)
                        this.AuthoriseAttribute = new AuthoriseAttribute();

                    this.AuthoriseAttribute
                            .CombineWith(__attribute.Value as AuthorizeAttribute);
                }
                else if (__attribute.Key == "AuthoriseAttribute")
                {
                    if (this.AuthoriseAttribute == null)
                        AuthoriseAttribute = __attribute.Value as AuthoriseAttribute;
                    else
                        AuthoriseAttribute.CombineWith(__attribute.Value as AuthoriseAttribute);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public AuthoriseAttribute AuthoriseAttribute { get; private set; }
    }
}
