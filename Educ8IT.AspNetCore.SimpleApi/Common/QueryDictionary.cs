// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Includes code adapted from Microsoft.AspNetCore.Routing.RouteValueDictionary

using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// Allows simple Dictionary setup from an anonymous object of KVP
    /// </summary>
    public class EasyDictionary : Dictionary<string, string>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="values"></param>
        public EasyDictionary(object values = null)
        {
            // START
            // Copyright (c) .NET Foundation. All rights reserved.
            // Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
            if (values is IEnumerable<KeyValuePair<string, string>> keyValueEnumerable)
            {
                foreach (var kvp in keyValueEnumerable)
                {
                    Add(kvp.Key, kvp.Value);
                }

                return;
            }
            // END
            else
            {
                var rvd = new RouteValueDictionary(values);
                foreach (var item in  rvd)
                {
                    this.Add(item.Key, item.Value?.ToString() ?? String.Empty);
                }
            }
        }

    }
}
