// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Authorisation.Attributes;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationAwareApiControllerItem : ApiControllerItem
    {
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

        private void ParseAuthorisationAttributes()
        {
            AuthoriseAttribute = null;

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "AuthorizeAttribute")
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

        private List<IApiMethodItem> _Methods = null;

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
    }
}
