// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ApiPropertyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiPropertyAttribute()
            : base()
        { }

        /// <summary>
        /// The Property will still be mapped, but the public documentation will not display this method.
        /// </summary>
        public bool ExcludeFromDocumentation { get; set; }

        /// <summary>
        /// The Property will still be mapped, but the public documentation will not display this method if their parent is in this set of Types.
        /// </summary>
        public Type[] ExcludeIfParentTypeIn { get; set; } = null;

        /// <summary>
        /// Used to remove Field name from an Update
        /// </summary>
        public string DbFieldName { get; set; }

        /// <summary>
        /// Marks this Property as Deprecated / Obsolete
        /// </summary>
        public bool IsDeprecated { get; set; }

        /// <summary>
        /// Tells the API user which Property to use in its place (if appropriate)
        /// </summary>
        public string DeprecationAdvice { get; set; }

    }
}
