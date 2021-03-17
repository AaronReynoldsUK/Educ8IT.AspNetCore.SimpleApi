// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Extensions.Options;

namespace Educ8IT.AspNetCore.SimpleApi.ApiDescriptions
{
    /// <summary>
    /// Default implementation of the abstract base class
    /// </summary>
    public class DefaultApiDescription : ApiDescription
    {
        #region Contructors

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        public DefaultApiDescription(
            IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
            : base (apiMapperOptions)
        { }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #endregion
    }
}
