// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Contains some code adapted from ASP.NET API

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Provide a readable name for the Type. Commonly used in documentation.
        /// TODO: this is HTML-friendly not output-agnostic. Either change the name or alter the function.
        /// </summary>
        /// <param name="type">A CLR type</param>
        /// <returns>The type name</returns>
        public static string GetReadableTypeName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            TypeNameAttribute modelNameAttribute = type.GetCustomAttribute<TypeNameAttribute>();
            if (modelNameAttribute != null && !String.IsNullOrEmpty(modelNameAttribute.Name))
            {
                return modelNameAttribute.Name;
            }

            string modelName = type.Name;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: GenericOfAgurment1AndArgument2
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string genericTypeName = genericType.Name;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                string[] argumentTypeNames = genericArguments.Select(t => GetReadableTypeName(t)).ToArray();

                //modelName = String.Format(CultureInfo.InvariantCulture, "{0}Of{1}", genericTypeName, String.Join("And", argumentTypeNames));
                modelName = String.Format(CultureInfo.InvariantCulture, "{0}&lt;{1}&gt;", genericTypeName, String.Join(",", argumentTypeNames));
            }

            return modelName;
        }

        /// <summary>
        /// Determine if this type is Nullable
        /// </summary>
        /// <param name="type">A CLR type</param>
        /// <returns>true if Nullable</returns>
        public static bool IsNullable(this Type type)
        {
            //if (type.IsValueType)
            //    return true;

            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Gets the underlying type from a Nullable.
        /// e.g. gets typeof(Int) when given typeof(Int?)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        /// <summary>
        /// Determine if this type is Enumerable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterface(nameof(IEnumerable)) != null;
        }

        /// <summary>
        /// Determine if this type is a Collection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type type)
        {
            return type.GetInterface(nameof(ICollection)) != null;
        }

        /// <summary>
        /// If this type is Enumerable and Generic, get the first Generic argument.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericEnumeratedType(this Type type)
        {
            return type.IsEnumerable() && type.IsGenericType
                ? type.GetGenericArguments()[0]
                : null;
        }

        /// <summary>
        /// Get the Enumerated type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNonGenericEnumeratedType(this Type type)
        {
            return type.IsArray
                ? type.GetElementType()
                : null;
        }

        /// <summary>
        /// Determine if this type is a Dictionary
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// If this type is a Dictionary, get the type of the Dictionary key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetDictionaryKeyType(this Type type)
        {
            return type.IsDictionary()
                ? type.GetGenericArguments()[0]
                : null;
        }

        /// <summary>
        /// If this type is a Dictionary, get the type of the Dictionary value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetDictionaryValueType(this Type type)
        {
            return type.IsDictionary()
                ? type.GetGenericArguments()[1]
                : null;
        }
    }
}
