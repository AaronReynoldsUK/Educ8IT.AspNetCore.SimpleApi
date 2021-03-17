// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
//using Microsoft.AspNetCore.Components;
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
    public class ApiControllerItem : TypeDescription, IApiControllerItem
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ApiControllerItem() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public ApiControllerItem(Type controller) : base(controller)
        {
            this.Type = controller;

            ParseAttributes();
        }

        #endregion

        #region Private Fields

        private List<IApiMethodItem> _Methods = null;
        private IDictionary<ApiVersion, List<IApiMethodItem>> _versionedMethods = null;

        #endregion

        #region Properties

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Type ControllerType { get { return base.Type; } private set { base.Type = value; } }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<KeyValuePair<string, Attribute>> Attributes { get; private set; }

        ///// <inheritdoc/>
        //[Obsolete("Use Name instead", true)]
        //public string RouteName
        //{
        //    get
        //    {
        //        var __name = Name;
        //        var __cn = "Controller";
        //        return __name.EndsWith(__cn, StringComparison.InvariantCultureIgnoreCase)
        //            ? __name.Substring(0, __name.Length - __cn.Length)
        //            : __name;
        //    }
        //}

        /// <inheritdoc/>
        public bool? Ignore { get; private set; }

        /// <inheritdoc/>
        public List<string> RoutePrefixes { get; private set; }

        /// <inheritdoc/>
        public bool? ExcludeFromDocumentation { get; private set; }

        /// <inheritdoc/>
        public List<ApiVersion> Versions { get; private set; }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public virtual List<IApiMethodItem> Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = GetMethods();

                return _Methods;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedRequestContentTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AllowedResponseContentTypes { get; private set; }

        #endregion

        #region Public Helper Methods

        /// <inheritdoc/>
        public void Document(IApiDescription apiDescription, IDocumentationProvider documentationProvider)
        {
            if (apiDescription == null)
                return;

            if (documentationProvider == null)
                return;

            this.Description =
                documentationProvider.GetSummaryOrDescription(ControllerType)
                ?? this.Description;

            apiDescription.GenerateTypeDescription(this.Type, documentationProvider);

            foreach (var apiMethodItem in this.Methods)
                apiMethodItem.Document(apiDescription, documentationProvider);
        }

        /// <inheritdoc/>
        public void SetDefaultVersion(ApiVersion apiDefaultVersion)
        {
            if (this.Versions == null)
                this.Versions = new List<ApiVersion>();

            if (this.Versions.Count == 0)
                this.Versions.Add(apiDefaultVersion);
        }

        /// <inheritdoc/>
        public IDictionary<ApiVersion, List<IApiMethodItem>> GetVersionedMethods(IApiMapperOptions apiMapperOptions)
        {
            if (!apiMapperOptions.UseVersioning)
                return null;

            if (_versionedMethods != null)
                return _versionedMethods;

            _versionedMethods = new Dictionary<ApiVersion, List<IApiMethodItem>>();

            if (this.Versions == null)
                this.Versions = new List<ApiVersion>();

            // Step 1: check for Default Api Version if required
            if (apiMapperOptions.AssumeDefaultVersionWhenUnspecified && apiMapperOptions.DefaultApiVersion == null)
                throw new Exception("DefaultApiVersion must be specified if AssumeDefaultVersionWhenUnspecified is set.");

            // Step 2: set default Version if required
            if (this.Versions.Count == 0 && apiMapperOptions.AssumeDefaultVersionWhenUnspecified)
            {
                this.Versions.Add(apiMapperOptions.DefaultApiVersion);
            }

            // Step 3: push all versions up to the Controller
            foreach (var __method in this.Methods)
            {
                if (__method.Versions != null && __method.Versions.Count > 0)
                {
                    foreach (var __methodVersion in __method.Versions)
                    {
                        // If Controller doesn't have the same Major and Minor version, add it
                        if (!this.Versions.Exists(cv => cv.EqualsSameMajorAndMinor(__methodVersion)))
                            this.Versions.Add(__methodVersion.FromMajorAndMinor());
                    }
                }
            }

            // Step 4: create Version tree
            this.Versions.ForEach((version) =>
            {
                _versionedMethods.Add(version, new List<IApiMethodItem>());
            });

            // Step 5: sort actions/methods into the Version tree (using Major and Minor only)
            foreach (var __method in this.Methods)
            {
                var __methodVersions = (__method.Versions != null && __method.Versions.Count > 0) ? __method.Versions : this.Versions;

                foreach (var __methodVersion in __methodVersions)
                {
                    _versionedMethods.First(e => e.Key.EqualsSameMajorAndMinor(__methodVersion))
                        .Value.Add(__method);
                }
            }

            return _versionedMethods;
        }

        /// <inheritdoc/>
        public virtual List<IApiMethodItem> GetMethods()
        {
            var __methodItems = new List<IApiMethodItem>();

            var __parentIgnoreSetting = Ignore;

            var __methods = this.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var __methodInfo in __methods)
            {
                if (ignoreMethodsOfName.Contains(__methodInfo.Name))
                    continue;

                if (__methodItems.Exists(m => m.Name.Equals(__methodInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception("You cannot have multiple Methods/Actions with the same name on a Controller.");

                var __apiMethodItem = new ApiMethodItem(__methodInfo, this);

                if (__apiMethodItem.Ignore.HasValue)
                {
                    if (__apiMethodItem.Ignore.Value)
                        continue;
                }
                else if (__parentIgnoreSetting.HasValue && __parentIgnoreSetting.Value)
                    continue;

                if (__apiMethodItem.MethodInfo.ReturnType == typeof(Task))
                    throw new Exception($"You cannot have a return type of void Task. Please update the method signature in {Name}:{__apiMethodItem.Name}");

                __methodItems.Add(__apiMethodItem);
            }

            return __methodItems;
        }

        /// <inheritdoc/>
        public IApiMethodItem GetApiMethodItem(string methodName)
        {
            if (this.Methods == null)
                return null;

            var __uniqueName = $"{this.Name}.{methodName}"; ;

            return Methods.FirstOrDefault(m => m.UniqueName == __uniqueName);
        }

        #endregion

        #region Private Helper Methods

        private void ParseAttributes()
        {
            Attributes = new List<KeyValuePair<string, Attribute>>();

            ControllerType.GetCustomAttributes().ToList()
                .ForEach(i => Attributes.Add(new KeyValuePair<string, Attribute>(i.GetType().Name, i)));

            AllowedRequestContentTypes = new List<string>();
            AllowedResponseContentTypes = new List<string>();

            ExcludeFromDocumentation = default;
            Ignore = default;
            //Name = 
            RoutePrefixes = new List<string>();
            Versions = new List<ApiVersion>();

            foreach (var __attribute in Attributes)
            {
                if (__attribute.Key == "IgnoreAttribute")
                {
                    Ignore = (__attribute.Value as IgnoreAttribute)?.ShouldIgnore;
                }
                else if (__attribute.Value is AllowedRequestContentTypeAttribute allowedRequestContentTypeAttribute)
                {
                    if (!AllowedRequestContentTypes.Contains(allowedRequestContentTypeAttribute.ContentType))
                        AllowedRequestContentTypes.Add(allowedRequestContentTypeAttribute.ContentType);
                }
                else if (__attribute.Value is AllowedResponseContentTypeAttribute allowedResponseContentTypeAttribute)
                {
                    if (!AllowedResponseContentTypes.Contains(allowedResponseContentTypeAttribute.ContentType))
                        AllowedResponseContentTypes.Add(allowedResponseContentTypeAttribute.ContentType);
                }
                //else if (__attribute.Key == "AllowedRequestContentTypeAttribute")
                //{                    
                //    AllowedRequestContentTypes.Add((__attribute.Value as AllowedRequestContentTypeAttribute).ContentType);
                //}
                //else if (__attribute.Key == "AllowedResponseContentTypeAttribute")
                //{
                //    AllowedResponseContentTypes.Add((__attribute.Value as AllowedResponseContentTypeAttribute).ContentType);
                //}
                else if (__attribute.Key == "ExcludeFromDocumentationAttribute")
                {
                    ExcludeFromDocumentation = (__attribute.Value as ExcludeFromDocumentationAttribute)?.ShouldExclude;
                }
                else if (__attribute.Key == "VersionAttribute")
                {
                    var __apiVersion = (__attribute.Value as VersionAttribute)?.Version;
                    if (__apiVersion != null)
                        Versions.Add(__apiVersion);
                }
                else if (__attribute.Key == "ApiControllerAttribute")
                {
                    var __apiCA = __attribute.Value as ApiControllerAttribute;
                    this.Name = __apiCA.Name;

                    var __routePrefixes = __apiCA?.RouteTemplates;
                    foreach (var __routePrefix in __routePrefixes)
                    {
                        if (!String.IsNullOrEmpty(__routePrefix))
                            RoutePrefixes.Add(__routePrefix.TrimEnd('/'));
                    }
                }
            }

            if (RoutePrefixes.Count == 0)
            {
                RoutePrefixes.Add("");
            }
        }

        #endregion

        private static string[] ignoreMethodsOfName = new string[]
        {
            "GetType",
            "ToString",
            "Equals",
            "GetHashCode"
        };
    }
}
