// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.ApiDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationAwareApiDescription : ApiDescription
    {
        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        public AuthenticationAwareApiDescription(
            IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
            : base(apiMapperOptions)
        { }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise AuthenticationAwareApiControllerItem from controller type
        /// </summary>
        /// <param name="controllerType">Controller class type</param>
        /// <returns>An AuthenticationAwareApiControllerItem object</returns>
        public override IApiControllerItem InitialiseController(Type controllerType)
        {
            return new AuthenticationAwareApiControllerItem(controllerType);
        }

        #endregion
    }
}
