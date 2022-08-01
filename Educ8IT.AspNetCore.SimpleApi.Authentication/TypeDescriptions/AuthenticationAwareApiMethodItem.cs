// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationAwareApiMethodItem : ApiMethodItem
    {
        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public AuthenticationAwareApiMethodItem() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="apiControllerItem"></param>
        public AuthenticationAwareApiMethodItem(MethodInfo methodInfo, IApiControllerItem apiControllerItem)
            : base(methodInfo, apiControllerItem)
        {
            ParseAuthenticationAttributes();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AuthenticateAttribute AuthenticateAttribute { get; private set; }

        #endregion

        private void ParseAuthenticationAttributes()
        {
            AuthenticateAttribute = null;

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "AllowAnonymousAttribute")
                {
                    AuthenticateAttribute = null;
                    break;
                }
                else if (__attribute.Key == "AuthenticateAttribute")
                {
                    if (AuthenticateAttribute == null)
                        AuthenticateAttribute = __attribute.Value as AuthenticateAttribute;
                    else
                        AuthenticateAttribute.CombineWith(__attribute.Value as AuthenticateAttribute);
                }
            }
        }
    }
}
