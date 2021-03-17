// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ApiPropertyItem : TypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiPropertyItem() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        public ApiPropertyItem(PropertyInfo propertyInfo)
        {
            if (Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyAliasAttribute)) is PropertyAliasAttribute propertyAliasAttribute)
                this.Alias = propertyAliasAttribute.Alias;

            this.IsEnumType = propertyInfo.PropertyType.BaseType == typeof(Enum);
            this.IsComplexType = !TypeInformation.SimpleTypes.ContainsKey(propertyInfo.PropertyType);
            this.Name = propertyInfo.Name;
            this.PropertyInfo = propertyInfo;
            this.Type = propertyInfo.PropertyType;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsComplexType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnumType { get; private set; }
    }
}
