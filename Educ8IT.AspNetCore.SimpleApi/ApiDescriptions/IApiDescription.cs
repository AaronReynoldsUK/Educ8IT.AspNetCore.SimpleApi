// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.ApiDescriptions
{
    /// <summary>
    /// A meta description of the API in terms of Controllers, Methods/Actions and Types.
    /// </summary>
    public interface IApiDescription
    {
        #region Properties

        /// <summary>
        /// Options that determine the working of the API system
        /// </summary>
        public IApiMapperOptions ApiMapperOptions { get; }

        /// <summary>
        /// A list of <see cref="IApiControllerItem"/>.
        /// These are class representations of the API controller classes.
        /// </summary>
        public List<IApiControllerItem> Controllers { get; }

        /// <summary>
        /// A list of <see cref="IApiMethodItem"/>.
        /// These are class representations of the API methods/actions.
        /// </summary>
        public List<IApiMethodItem> Methods { get; }

        /// <summary>
        /// A collections of the <see cref="TypeDescription"/> objects representing Types used by the API.
        /// Mostly used in documentation.
        /// </summary>
        public Dictionary<string, TypeDescription> GeneratedModels { get; }

        /// <summary>
        /// A list of <see cref="ApiVersion"/> versions supported by the API.
        /// </summary>
        public List<ApiVersion> Versions { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear the Controllers and Actions/Methods so the documentation will be rebuilt.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Get an <see cref="IApiMethodItem"/> by controller name and method name.
        /// </summary>
        /// <param name="controllerName">The name of the parent controller for the Method</param>
        /// <param name="methodName">The name of the Method to return</param>
        /// <returns>An <see cref="IApiMethodItem"/> representation of the Method found</returns>
        public IApiMethodItem GetApiMethodItem(string controllerName, string methodName);

        /// <summary>
        /// Populate and return an initialised list of IApiControllerItem
        /// </summary>
        /// <returns></returns>
        public List<IApiControllerItem> GetControllersAndMethods();

        /// <summary>
        /// Initialise ApiControllerItem from controller type
        /// </summary>
        /// <param name="controllerType">Controller class type</param>
        /// <returns>An ApiControllerItem object</returns>
        public abstract IApiControllerItem InitialiseController(Type controllerType);

        /// <summary>
        /// Get a collection of the controllers and methods organised by <see cref="ApiVersion"/>.
        /// </summary>
        /// <returns>Dictionary of controllers, keyed by version. Each version set contains all controllers/methods supported by that version.</returns>
        public IDictionary<ApiVersion, List<IApiControllerItem>> GetVersionedControllers();

        /// <summary>
        /// Returns a matching <see cref="TypeDescription"/> object for a CLR type.
        /// </summary>
        /// <param name="type">The CLR type to look up.</param>
        /// <returns>The matching type description</returns>
        public TypeDescription GetTypeDescription(Type type);

        /// <summary>
        /// Generate and return the <see cref="TypeDescription"/> for a CLR type.
        /// </summary>
        /// <param name="type">The CLR type to generate a type description for.</param>
        /// <param name="documentationProvider">The documentation provider to use in generation.</param>
        /// <returns>The matching type description</returns>
        public TypeDescription GenerateTypeDescription(Type type, IDocumentationProvider documentationProvider);

        /// <summary>
        /// Get a CLR object of the specified type.
        /// This searches all documentation providers for a match.
        /// </summary>
        /// <param name="typeFullName">The name of the type to find and return. Types are checked by full name, then name.</param>
        /// <returns>The matching CLR type if found.</returns>
        public Type GetTypeByName(string typeFullName);

        /// <summary>
        /// Get a CLR object of the specified type.
        /// This searches all documentation providers for a match.
        /// </summary>
        /// <param name="typeFullName">The name of the type to find and return. Types are checked by full name, then name.</param>
        /// <param name="documentationProvider">The first documentation provider that matched.</param>
        /// <returns>The matching CLR type if found.</returns>
        public Type GetTypeByName(string typeFullName, out IDocumentationProvider documentationProvider);

        #endregion
    }
}
