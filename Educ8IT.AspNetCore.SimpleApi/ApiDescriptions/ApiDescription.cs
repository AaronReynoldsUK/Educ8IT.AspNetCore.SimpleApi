// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.ApiDescriptions
{
    /// <summary>
    /// Abstract implementation of <see cref="IApiDescription"/>.
    /// Contains the basic functionality so overloaded versions can be used easily.
    /// </summary>
    public abstract class ApiDescription : IApiDescription
    {
        #region Private Fields

        private List<IApiControllerItem> _controllers = null;
        private List<IApiMethodItem> _methods = null;
        private IDictionary<ApiVersion, List<IApiControllerItem>> _versionedControllers = null;

        #endregion

        #region Contructors

        /// <summary>
        /// Initialise the ApiDescription with a copy of the <see cref="ApiMapperOptions"/>
        /// </summary>
        public ApiDescription(IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
        {
            ApiMapperOptions = apiMapperOptions?.CurrentValue ?? throw new ArgumentNullException(nameof(apiMapperOptions));

            GeneratedModels = new Dictionary<string, TypeDescription>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public IApiMapperOptions ApiMapperOptions { get; } = null;

        /// <inheritdoc/>
        public List<IApiControllerItem> Controllers
        {
            get
            {
                if (_controllers == null)
                {
                    _controllers = new List<IApiControllerItem>();
                    _controllers = GetControllersAndMethods();
                }   

                return  _controllers;
            }
        }

        /// <inheritdoc/>
        public List<IApiMethodItem> Methods
        {
            get
            {
                if (Controllers == null)
                    return null;

                if (_methods == null)
                {
                    _methods = new List<IApiMethodItem>();

                    Controllers.ForEach((controller) =>
                    {
                        _methods.AddRange(controller.Methods);
                    });
                }

                return _methods;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, TypeDescription> GeneratedModels { get; private set; }

        /// <inheritdoc/>
        public List<ApiVersion> Versions
        {
            get
            {
                return _versionedControllers?.Keys?.ToList();
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Reset()
        {
            _controllers = null;
            _methods = null;
        }

        /// <inheritdoc/>
        public IApiMethodItem GetApiMethodItem(string controllerName, string methodName)
        {
            if (Controllers == null || Methods == null)
                return null;

            var __uniqueName = $"{controllerName}.{methodName}"; ;

            return Methods.FirstOrDefault(m => m.UniqueName == __uniqueName);
        }

        /// <inheritdoc/>
        public virtual List<IApiControllerItem> GetControllersAndMethods()
        {
            foreach (var __documentationProvider in ApiMapperOptions.DocumentationProviders)
            {
                if (__documentationProvider.ApiAssembly == null)
                    throw new InvalidOperationException("The API Assembly was not passed.");

                // By standard we include class types that end with "Controller"
                static bool defaultPredicate(Type t) => t.Name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase);

                var __controllers =
                    from type in __documentationProvider.ApiAssembly.GetTypes()
                        .Where(defaultPredicate)
                    select type;

                if (ApiMapperOptions.ControllerBaseType != null)
                {
                    // If we've specified a base type or interface, let's include these in the controllers
                    var __additionalControllers =
                        from type in __documentationProvider.ApiAssembly.GetTypes()
                            .Where(t => t.IsAssignableFrom(ApiMapperOptions.ControllerBaseType))
                        select type;

                    __controllers = __controllers.Concat(__additionalControllers);
                }

                if (__controllers != null)
                {
                    foreach (var __controller in __controllers)
                    {
                        var __apiControllerItem = InitialiseController(__controller);

                        __apiControllerItem.Document(this, __documentationProvider);

                        if (ApiMapperOptions.UseVersioning
                            && ApiMapperOptions.DefaultApiVersion != null
                            && ApiMapperOptions.AssumeDefaultVersionWhenUnspecified)
                        {
                            __apiControllerItem.SetDefaultVersion(ApiMapperOptions.DefaultApiVersion);
                        }

                        if (__apiControllerItem.Methods.Count == 0 && (__apiControllerItem.Ignore ?? false))
                        { }
                        else
                        {
                            Controllers.Add(__apiControllerItem);
                        }
                    }
                }
            }

            return Controllers;
        }

        /// <inheritdoc/>
        public virtual IApiControllerItem InitialiseController(Type controllerType)
        {
            return new ApiControllerItem(controllerType);
        }

        /// <inheritdoc/>
        public IDictionary<ApiVersion, List<IApiControllerItem>> GetVersionedControllers()
        {
            if (!ApiMapperOptions.UseVersioning)
                return null;

            if (_versionedControllers != null)
                return _versionedControllers;

            _versionedControllers = new Dictionary<ApiVersion, List<IApiControllerItem>>();

            // build the Version tree for each Controller
            this.Controllers.ForEach((controller) =>
            {
                controller.GetVersionedMethods(ApiMapperOptions);

                foreach (var __controllerVersion in controller.Versions)
                {
                    // Add Version to Version tree if not exists
                    if (!_versionedControllers.Keys.ToList().Exists(v => v.EqualsSameMajorAndMinor(__controllerVersion)))
                    {
                        _versionedControllers.Add(__controllerVersion.FromMajorAndMinor(), new List<IApiControllerItem>());
                    }

                    // Add Controller to Version tree
                    _versionedControllers.First(e => e.Key.EqualsSameMajorAndMinor(__controllerVersion))
                        .Value.Add(controller);
                }
            });

            return _versionedControllers;
        }

        /// <inheritdoc/>
        public TypeDescription GetTypeDescription(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Get underlying type in the case of Nullables
            type = Nullable.GetUnderlyingType(type) ?? type;

            string typeName = type.GetReadableTypeName();

            if (this.GeneratedModels.TryGetValue(typeName, out TypeDescription typeDescription))
            {
                if (type != typeDescription.Type)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentCulture,
                            "A model description could not be created. Duplicate model name '{0}' was found for types '{1}' and '{2}'. " +
                            "Use the [ModelName] attribute to change the model name for at least one of the types so that it has a unique name.",
                            typeName,
                            typeDescription.Type.FullName,
                            type.FullName));
                }

                return typeDescription;
            }

            return null;

            //foreach (var __documentationProvider in ApiMapperOptions.DocumentationProviders)
            //{
            //    typeDescription = GenerateTypeDescription(type, __documentationProvider);
            //    if (typeDescription != null)
            //        break;
            //}
            //return typeDescription;
        }

        /// <inheritdoc/>
        public TypeDescription GenerateTypeDescription(Type type, IDocumentationProvider documentationProvider)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            // Get underlying type in the case of Nullables
            type = Nullable.GetUnderlyingType(type) ?? type;

            // Check if already exists
            TypeDescription typeDescription = GetTypeDescription(type);
            if (typeDescription != null)
                return typeDescription;

            if (TypeInformation.SimpleTypes.ContainsKey(type))
                return GenerateSimpleTypeDescription(type, documentationProvider);

            else if (type.IsEnum)
                return GenerateEnumTypeDescription(type, documentationProvider);

            if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();

                if (genericArguments.Length == 1)
                {
                    Type enumerableType = typeof(IEnumerable<>).MakeGenericType(genericArguments);
                    if (enumerableType.IsAssignableFrom(type))
                    {
                        return GenerateCollectionModelDescription(type, genericArguments[0], documentationProvider);
                    }
                }
                if (genericArguments.Length == 2)
                {
                    Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(genericArguments);
                    if (dictionaryType.IsAssignableFrom(type))
                    {
                        return GenerateDictionaryModelDescription(type, genericArguments[0], genericArguments[1], documentationProvider);
                    }

                    Type keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(genericArguments);
                    if (keyValuePairType.IsAssignableFrom(type))
                    {
                        return GenerateKeyValuePairModelDescription(type, genericArguments[0], genericArguments[1], documentationProvider);
                    }
                }
            }

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                return GenerateCollectionModelDescription(type, elementType, documentationProvider);
            }

            if (type == typeof(NameValueCollection))
            {
                return GenerateDictionaryModelDescription(type, typeof(string), typeof(string), documentationProvider);
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return GenerateDictionaryModelDescription(type, typeof(object), typeof(object), documentationProvider);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return GenerateCollectionModelDescription(type, typeof(object), documentationProvider);
            }

            return GenerateComplexTypeModelDescription(type, documentationProvider);
        }

        /// <inheritdoc/>
        public Type GetTypeByName(string typeFullName)
        {
            Type __type = null;
            foreach (var __documentationProvider in ApiMapperOptions.DocumentationProviders)
            {
                __type = __documentationProvider.ApiAssembly.GetTypes()
                    .Where(t => t.FullName == typeFullName)?.FirstOrDefault()
                    ??
                    __documentationProvider.ApiAssembly.GetTypes()
                    .Where(t => t.Name == typeFullName)?.FirstOrDefault()
                    ?? null;

                if (__type != null)
                    break;
            }

            return __type;
        }

        /// <inheritdoc/>
        public Type GetTypeByName(string typeFullName, out IDocumentationProvider documentationProvider)
        {
            Type __type = null;
            documentationProvider = null;

            foreach (var __documentationProvider in ApiMapperOptions.DocumentationProviders)
            {
                documentationProvider = __documentationProvider;

                __type = __documentationProvider.ApiAssembly.GetTypes()
                    .Where(t => t.FullName == typeFullName)?.FirstOrDefault()
                    ??
                    __documentationProvider.ApiAssembly.GetTypes()
                    .Where(t => t.Name == typeFullName)?.FirstOrDefault()
                    ?? null;

                if (__type != null)
                    break;
            }

            if (__type == null)
                documentationProvider = null;

            return __type;
        }
        
        #endregion

        #region Generate Type Descriptions

        private string CreateDefaultTypeDescription(Type type, IDocumentationProvider documentationProvider)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            if (TypeInformation.SimpleTypes.TryGetValue(type, out string __description))
                return __description;

            __description = documentationProvider.GetSummaryOrDescription(type);

            return __description;
        }

        private TypeDescription GenerateSimpleTypeDescription(Type type, IDocumentationProvider documentationProvider)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            SimpleTypeDescription simpleTypeDescription = new SimpleTypeDescription()
            {
                Name = type.GetReadableTypeName(),
                Type = type,
                Description = CreateDefaultTypeDescription(type, documentationProvider)
            };
            GeneratedModels.Add(simpleTypeDescription.Name, simpleTypeDescription);

            return simpleTypeDescription;
        }

        private EnumTypeDescription GenerateEnumTypeDescription(Type type, IDocumentationProvider documentationProvider)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            EnumTypeDescription enumTypeDescription = new EnumTypeDescription()
            {
                Name = type.GetReadableTypeName(),
                Type = type,
                Description = CreateDefaultTypeDescription(type, documentationProvider)
            };

            bool hasDataContractAttribute = type.GetCustomAttribute<DataContractAttribute>() != null;
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (ShouldDisplayMember(field, hasDataContractAttribute))
                {
                    EnumValueDescription enumValue = new EnumValueDescription
                    {
                        Documentation = documentationProvider.GetSummaryOrDescription(field),
                        Name = field.Name,
                        Value = field.GetRawConstantValue().ToString()
                    };

                    enumTypeDescription.Values.Add(enumValue);
                }
            }
            GeneratedModels.Add(enumTypeDescription.Name, enumTypeDescription);

            return enumTypeDescription;
        }

        private CollectionDescription GenerateCollectionModelDescription(Type modelType, Type elementType, IDocumentationProvider documentationProvider)
        {
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            if (elementType == null)
                throw new ArgumentNullException(nameof(elementType));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            TypeDescription collectionModelDescription = GenerateTypeDescription(elementType, documentationProvider);

            if (collectionModelDescription != null)
            {
                return new CollectionDescription
                {
                    Name = modelType.GetReadableTypeName(),
                    Type = modelType,
                    ElementDescription = collectionModelDescription
                };
            }

            return null;
        }

        private DictionaryDescription GenerateDictionaryModelDescription(Type modelType, Type keyType, Type valueType, IDocumentationProvider documentationProvider)
        {
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            if (keyType == null)
                throw new ArgumentNullException(nameof(keyType));

            if (valueType == null)
                throw new ArgumentNullException(nameof(valueType));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            TypeDescription keyModelDescription = GenerateTypeDescription(keyType, documentationProvider);
            TypeDescription valueModelDescription = GenerateTypeDescription(valueType, documentationProvider);

            return new DictionaryDescription
            {
                Name = modelType.GetReadableTypeName(),
                Type = modelType,
                KeyDescription = keyModelDescription,
                ValueDescription = valueModelDescription
            };
        }

        private KeyValuePairDescription GenerateKeyValuePairModelDescription(Type modelType, Type keyType, Type valueType, IDocumentationProvider documentationProvider)
        {
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            if (keyType == null)
                throw new ArgumentNullException(nameof(keyType));

            if (valueType == null)
                throw new ArgumentNullException(nameof(valueType));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            TypeDescription keyModelDescription = GenerateTypeDescription(keyType, documentationProvider);
            TypeDescription valueModelDescription = GenerateTypeDescription(valueType, documentationProvider);

            return new KeyValuePairDescription
            {
                Name = modelType.GetReadableTypeName(),
                Type = modelType,
                KeyDescription = keyModelDescription,
                ValueDescription = valueModelDescription
            };
        }

        private TypeDescription GenerateComplexTypeModelDescription(Type modelType, IDocumentationProvider documentationProvider)
        {
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));

            if (documentationProvider == null)
                throw new ArgumentNullException(nameof(documentationProvider));

            ComplexTypeDescription complexTypeDescription = new ComplexTypeDescription
            {
                Name = modelType.GetReadableTypeName(),
                Type = modelType,
                Description = CreateDefaultTypeDescription(modelType, documentationProvider)
            };
                        
            bool hasDataContractAttribute = modelType.GetCustomAttribute<DataContractAttribute>() != null;
            PropertyInfo[] properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                if (ShouldDisplayMember(property, hasDataContractAttribute))
                {
                    ApiPropertyItem apiPropertyItem = new ApiPropertyItem(property)
                    {
                        Description = documentationProvider.GetSummaryOrDescription(property)
                    };
                    complexTypeDescription.Properties.Add(apiPropertyItem);
                }
            }

            GeneratedModels.Add(complexTypeDescription.Name, complexTypeDescription);
            return complexTypeDescription;
        }

        private static bool ShouldDisplayMember(MemberInfo member, bool hasDataContractAttribute)
        {
            IgnoreAttribute ignore = member.GetCustomAttribute<IgnoreAttribute>();
            JsonIgnoreAttribute jsonIgnore = member.GetCustomAttribute<JsonIgnoreAttribute>();
            XmlIgnoreAttribute xmlIgnore = member.GetCustomAttribute<XmlIgnoreAttribute>();
            IgnoreDataMemberAttribute ignoreDataMember = member.GetCustomAttribute<IgnoreDataMemberAttribute>();
            NonSerializedAttribute nonSerialized = member.GetCustomAttribute<NonSerializedAttribute>();

            bool hasMemberAttribute = member.DeclaringType.IsEnum ?
                member.GetCustomAttribute<EnumMemberAttribute>() != null :
                member.GetCustomAttribute<DataMemberAttribute>() != null;

            // Display member only if all the followings are true:
            // no JsonIgnoreAttribute
            // no XmlIgnoreAttribute
            // no IgnoreDataMemberAttribute
            // no NonSerializedAttribute
            // no ApiExplorerSettingsAttribute with IgnoreApi set to true
            // no DataContractAttribute without DataMemberAttribute or EnumMemberAttribute
            return ignore == null &&
                jsonIgnore == null &&
                xmlIgnore == null &&
                ignoreDataMember == null &&
                nonSerialized == null &&
                (!hasDataContractAttribute || hasMemberAttribute);
        }

        #endregion
    }
}
