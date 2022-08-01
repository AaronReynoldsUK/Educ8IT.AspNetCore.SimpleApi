// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation;
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
    public class AuthorisationAwareApiControllerItem : AuthenticationAwareApiControllerItem
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public AuthorisationAwareApiControllerItem() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public AuthorisationAwareApiControllerItem(Type controller) : base(controller)
        {
            ParseAuthorisationAttributes();
        }

        #endregion

        #region Fields

        private List<IApiMethodItem> _Methods = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AuthoriseAttribute AuthoriseAttribute { get; private set; }

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
                __listOut.Add(new AuthorisationAwareApiMethodItem(__methodItem.MethodInfo, this));
            }

            return __listOut;
        }

        #endregion

        #region Private Methods

        private void ParseAuthorisationAttributes()
        {
            AuthoriseAttribute = null;

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "AuthorizeAttribute")
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

        #endregion
    }
}
