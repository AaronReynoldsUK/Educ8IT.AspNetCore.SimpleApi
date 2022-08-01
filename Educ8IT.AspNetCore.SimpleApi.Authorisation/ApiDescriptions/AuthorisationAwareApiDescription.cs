// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.ApiDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationAwareApiDescription : ApiDescription
    {
        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        public AuthorisationAwareApiDescription(
            IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
            : base(apiMapperOptions)
        { }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise AuthorisationAwareApiControllerItem from controller type
        /// </summary>
        /// <param name="controllerType">Controller class type</param>
        /// <returns>An AuthorisationAwareApiControllerItem object</returns>
        public override IApiControllerItem InitialiseController(Type controllerType)
        {
            return new AuthorisationAwareApiControllerItem(controllerType);
        }

        #endregion
    }
}
