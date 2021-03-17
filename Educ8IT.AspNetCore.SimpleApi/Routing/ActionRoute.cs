// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Routing.Patterns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ActionRoute
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method
        {
            get
            {
                return HttpMethod?.Method ?? "Unknown";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ParentRoutePrefixes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerRoutePrefix"></param>
        /// <param name="defaults"></param>
        /// <param name="parameterPolicies"></param>
        /// <param name="requiredValues"></param>
        /// <returns></returns>
        public RoutePattern GetRoutePattern(string controllerRoutePrefix = null, 
            object defaults = null, object parameterPolicies = null, object requiredValues = null)
        {
            var __prefix = controllerRoutePrefix ?? String.Empty;
            var __pattern = this.Pattern ?? String.Empty;

            if (__prefix.Length > 0 && __pattern.Length > 0)
                __prefix += "/";

            return RoutePatternFactory.Parse(__prefix + __pattern, defaults, parameterPolicies, requiredValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (ParentRoutePrefixes == null || ParentRoutePrefixes.Count == 0)
            {
                return $"{Order}:{HttpMethod} {Name}() => \"{Pattern ?? String.Empty}\"".Replace("//", "/");
            }
            else
            {
                List<string> __fullPatterns = new List<string>();
                foreach (var __parentRoutePrefix in ParentRoutePrefixes)
                {
                    var __prefix = __parentRoutePrefix ?? String.Empty;
                    var __pattern = this.Pattern ?? String.Empty;

                    if (__prefix.Length > 0 && __pattern.Length > 0)
                        __prefix += "/";

                    __fullPatterns.Add($"({HttpMethod}) {Order}:{Name} => \"{__prefix}{Pattern ?? String.Empty}\"".Replace("//", "/"));
                }
                return String.Join("\r\n", __fullPatterns);
            }
        }

    }
}
