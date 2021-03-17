// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class TypeDescription : ITypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public TypeDescription() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public TypeDescription(Type type) : this()
        {
            this.Name = type.Name;
            this.Type = type;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public TypeDescription(Type type, string name) : this()
        {
            this.Name = name;
            this.Type = type;

        }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Type Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName
        {
            get { return Type.GetReadableTypeName(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
