// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiMapperService
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiMapperOptions ApiMapperOptions { get; }

        /// <summary>
        /// 
        /// </summary>
        public IApiDescription ApiDescription { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<IApiControllerItem> Controllers { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<IApiMethodItem> Methods { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiControllerItem"></param>
        /// <param name="apiMethodItem"></param>
        /// <returns></returns>
        public RequestDelegate GetEndpointDelegateProxy(IApiControllerItem apiControllerItem, IApiMethodItem apiMethodItem);
    }
}
