// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ComplexTypeDescription : TypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public ComplexTypeDescription()
        {
            Properties = new Collection<ApiPropertyItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<ApiPropertyItem> Properties { get; private set; }
    }
}
