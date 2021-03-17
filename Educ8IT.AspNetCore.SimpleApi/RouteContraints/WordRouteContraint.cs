// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.RouteContraints
{
    /// <summary>
    /// 
    /// </summary>
    public class WordRouteContraint : IRouteConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RouteConstraintKey = "word";

        private static Regex WordRegex = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="route"></param>
        /// <param name="routeKey"></param>
        /// <param name="values"></param>
        /// <param name="routeDirection"></param>
        /// <returns></returns>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (route == null)
                throw new ArgumentNullException(nameof(route));

            if (routeKey == null)
                throw new ArgumentNullException(nameof(routeKey));

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (WordRegex == null)
            {
                WordRegex = new Regex(@"^\w*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            if (values.TryGetValue(routeKey, out object routeValue))
            {
                var parameterValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
                return WordRegex.IsMatch(parameterValueString);
            }

            return false;
        }
    }
}
