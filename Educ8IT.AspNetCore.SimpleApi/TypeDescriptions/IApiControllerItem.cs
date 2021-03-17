// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
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
    public interface IApiControllerItem : ITypeDescription
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Type ControllerType { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<KeyValuePair<string, Attribute>> Attributes { get; }

        ///// <summary>
        ///// Used in RoutePattern matching
        ///// </summary>
        //public string RouteName { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool? Ignore { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> RoutePrefixes { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ExcludeFromDocumentation { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ApiVersion> Versions { get; }

        /// <inheritdoc/>
        //[System.Text.Json.Serialization.JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        //[XmlIgnore]
        //[IgnoreDataMember]
        public List<IApiMethodItem> Methods { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedRequestContentTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedResponseContentTypes { get; }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <param name="documentationProvider"></param>
        public void Document(IApiDescription apiDescription, IDocumentationProvider documentationProvider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiDefaultVersion"></param>
        public void SetDefaultVersion(ApiVersion apiDefaultVersion);

        /// <summary>
        /// A method could have several versions but also there could be several methods (each with a different version) for an endpoint.
        /// All we are concerned with here is the versions for each method.
        /// </summary>
        /// <param name="apiMapperOptions"></param>
        /// <returns></returns>
        public IDictionary<ApiVersion, List<IApiMethodItem>> GetVersionedMethods(IApiMapperOptions apiMapperOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IApiMethodItem> GetMethods();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public IApiMethodItem GetApiMethodItem(string methodName);

        #endregion
    }
}
