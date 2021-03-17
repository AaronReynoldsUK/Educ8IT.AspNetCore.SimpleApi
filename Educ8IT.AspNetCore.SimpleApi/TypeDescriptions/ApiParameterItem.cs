// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Documentation;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
    public class ApiParameterItem : TypeDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiParameterItem() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        public ApiParameterItem(ParameterInfo parameterInfo)
        {
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromContextAttribute)) is FromContextAttribute fromContextAttribute)
                this.FromContextAttribute = fromContextAttribute;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromHeaderAttribute)) is FromHeaderAttribute fromHeaderAttribute)
                this.FromHeaderAttribute = fromHeaderAttribute;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromRouteAttribute)) is FromRouteAttribute fromRouteAttribute)
                this.FromRouteAttribute = fromRouteAttribute;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromBodyAttribute)) is FromBodyAttribute fromBodyAttribute)
                this.FromBodyAttribute = fromBodyAttribute;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromFormAttribute)) is FromFormAttribute fromFormAttribute)
                this.FromFormAttribute = fromFormAttribute;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(FromQueryAttribute)) is FromQueryAttribute fromQueryAttribute)
                this.FromQueryAttribute = fromQueryAttribute;

            this.IsEnumType = parameterInfo.ParameterType.BaseType == typeof(Enum);
            this.IsComplexType = !TypeInformation.SimpleTypes.ContainsKey(parameterInfo.ParameterType);
            this.Name = parameterInfo.Name;
            this.ParameterInfo = parameterInfo;
            this.Position = parameterInfo.Position;
            this.Type = parameterInfo.ParameterType;

            // Added to allow Parameter Attributes in the Documentation
            this.GenerateAnnotations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <param name="documentationProvider"></param>
        public void Document(IApiDescription apiDescription, IDocumentationProvider documentationProvider)
        {
            if (documentationProvider == null)
                return;

            this.Description =
                documentationProvider.GetSummaryOrDescription(ParameterInfo)
                ?? this.Description;

            apiDescription.GenerateTypeDescription(this.Type, documentationProvider);
        }

        #region Attributes

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromContextAttribute FromContextAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromHeaderAttribute FromHeaderAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromRouteAttribute FromRouteAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromBodyAttribute FromBodyAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromFormAttribute FromFormAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public FromQueryAttribute FromQueryAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsContextParameter { get { return FromContextAttribute != null; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHeaderParameter { get { return FromHeaderAttribute != null; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRouteParameter { get { return FromRouteAttribute != null; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBodyParameter { get { return FromBodyAttribute != null; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFormParameter { get { return FromFormAttribute != null; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsQueryParameter
        {
            get
            {
                return FromQueryAttribute != null
                    || !(IsContextParameter || IsBodyParameter || IsHeaderParameter || IsRouteParameter || IsFormParameter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EParameterFrom ParameterFrom
        {
            get
            {
                if (IsContextParameter)
                    return EParameterFrom.Context;
                else if (IsHeaderParameter)
                    return EParameterFrom.Header;
                else if (IsRouteParameter)
                    return EParameterFrom.Route;
                else if (IsBodyParameter)
                    return EParameterFrom.Body;
                else if (IsFormParameter)
                    return EParameterFrom.Form;
                else return EParameterFrom.Query;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterFromText
        {
            get { return ParameterFrom.ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            get
            {
                if (IsContextParameter)
                    return FromContextAttribute?.ContextItemName ?? null;
                else if (IsHeaderParameter)
                    return FromHeaderAttribute?.Name ?? null;
                else if (IsRouteParameter)
                    return FromRouteAttribute?.Alias ?? null;
                else if (IsBodyParameter)
                    return FromBodyAttribute?.Alias ?? null;
                else if (IsFormParameter)
                    return FromFormAttribute?.Alias ?? null;
                else if (IsQueryParameter)
                    return FromQueryAttribute?.Alias ?? null;
                else
                    return null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Collection<ParameterAnnotation> Annotations { get; private set; } = new Collection<ParameterAnnotation>();

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ParameterInfo ParameterInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsComplexType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnumType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequired
        {
            get
            {
                var __attribute = Attribute.GetCustomAttribute(ParameterInfo, typeof(RequiredAttribute)) as RequiredAttribute;
                return __attribute != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength
        {
            get
            {
                var __attribute = Attribute.GetCustomAttribute(ParameterInfo, typeof(MaxLengthAttribute)) as MaxLengthAttribute;

                if (__attribute != null)
                    return __attribute.Length;

                var __attributeSL = Attribute.GetCustomAttribute(ParameterInfo, typeof(StringLengthAttribute)) as StringLengthAttribute;

                return __attributeSL?.MaximumLength ?? default(int?);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? MinLength
        {
            get
            {
                var __attribute = Attribute.GetCustomAttribute(ParameterInfo, typeof(MinLengthAttribute)) as MinLengthAttribute;

                if (__attribute != null)
                    return __attribute.Length;

                var __attributeSL = Attribute.GetCustomAttribute(ParameterInfo, typeof(StringLengthAttribute)) as StringLengthAttribute;

                return __attributeSL?.MinimumLength ?? default(int?);
            }
        }

        private List<ApiPropertyItem> _SubProperties = null;

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<ApiPropertyItem> SubProperties
        {
            get
            {
                if (_SubProperties == null)
                {
                    _SubProperties = new List<ApiPropertyItem>();
                    var __properties = this.Type
                        .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                        .ToList();

                    __properties.ForEach(prop =>
                    {
                        _SubProperties.Add(new ApiPropertyItem(prop));
                    });
                }
                return _SubProperties;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} ({1})", this.Name, this.Type.Name);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        public object GetInstanceFromNVC(NameValueCollection nameValueCollection)
        {
            try
            {
                object __instance = Activator.CreateInstance(this.Type);

                //var __properties = parameterItem.Type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (this.SubProperties != null)
                {
                    foreach (var __property in this.SubProperties)
                    {
                        var __keyMatch = nameValueCollection.AllKeys
                            .Where(k => k.ToLowerInvariant() == (this.Alias ?? __property.Name).ToLowerInvariant()).FirstOrDefault();

                        __keyMatch = __keyMatch ?? nameValueCollection.AllKeys
                            .Where(k => k.ToLowerInvariant() == (__property.Alias ?? __property.Name).ToLowerInvariant()).FirstOrDefault();

                        if (__keyMatch != null)
                            __property.PropertyInfo.SetValue(__instance, nameValueCollection[__keyMatch]);
                    }
                }

                return __instance;
            }
            catch { }

            return null;
        }
    }
}
