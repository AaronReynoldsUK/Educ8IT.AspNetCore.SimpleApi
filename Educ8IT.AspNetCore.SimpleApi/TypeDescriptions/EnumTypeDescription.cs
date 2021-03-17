// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumTypeDescription : TypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public EnumTypeDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<EnumValueDescription> Values { get; private set; }
    }
}
