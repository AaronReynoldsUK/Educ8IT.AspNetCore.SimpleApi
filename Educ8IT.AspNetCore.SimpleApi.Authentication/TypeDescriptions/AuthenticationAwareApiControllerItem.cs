// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationAwareApiControllerItem : ApiControllerItem
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public AuthenticationAwareApiControllerItem() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public AuthenticationAwareApiControllerItem(Type controller) : base(controller)
        {
            ParseAuthenticationAttributes();
        }

        #endregion

        #region Fields

        private List<IApiMethodItem> _Methods = null;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AuthenticateAttribute AuthenticateAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override List<IApiMethodItem> Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = GetMethods();

                return _Methods;
            }
        }

        /// <inheritdoc/>
        public override List<IApiMethodItem> GetMethods()
        {
            var __listIn = base.GetMethods();
            var __listOut = new List<IApiMethodItem>();

            foreach (var __methodItem in __listIn)
            {
                __listOut.Add(new AuthenticationAwareApiMethodItem(__methodItem.MethodInfo, this));
            }

            return __listOut;
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
