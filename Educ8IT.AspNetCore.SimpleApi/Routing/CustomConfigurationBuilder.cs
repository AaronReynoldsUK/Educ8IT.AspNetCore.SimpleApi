// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomConfigurationBuilder
    {
        private readonly CustomEndpointDataSource _dataSource;

        internal CustomConfigurationBuilder(CustomEndpointDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        //public void AddPattern(string pattern)
        //{
        //    AddPattern(RoutePatternFactory.Parse(pattern));
        //}

        //public void AddPattern(RoutePattern pattern)
        //{
        //    _dataSource.Patterns.Add(pattern);
        //}

        //public void AddHubMethod(string hub, string method, RequestDelegate requestDelegate)
        //{
        //    _dataSource.HubMethods.Add(new HubMethod
        //    {
        //        Hub = hub,
        //        Method = method,
        //        RequestDelegate = requestDelegate
        //    });
        //}
    }
}
