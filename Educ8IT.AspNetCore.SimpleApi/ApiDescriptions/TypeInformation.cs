// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.
// Contains some code adapted from ASP.NET API

using Educ8IT.AspNetCore.SimpleApi.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeInformation
    {
        /// <summary>
        /// 
        /// ... Modify this to add more default documentations
        /// </summary>
        public readonly static IDictionary<Type, string> SimpleTypes = new Dictionary<Type, string>
        {
            { typeof(Int16), "integer" },
            { typeof(Int32), "integer" },
            { typeof(Int64), "integer" },
            { typeof(UInt16), "unsigned integer" },
            { typeof(UInt32), "unsigned integer" },
            { typeof(UInt64), "unsigned integer" },
            { typeof(Byte), "byte" },
            { typeof(Char), "character" },
            { typeof(SByte), "signed byte" },
            { typeof(Uri), "URI" },
            { typeof(Single), "decimal number" },
            { typeof(Double), "decimal number" },
            { typeof(Decimal), "decimal number" },
            { typeof(String), "string" },
            { typeof(Guid), "globally unique identifier" },
            { typeof(TimeSpan), "time interval" },
            { typeof(DateTime), "date" },
            { typeof(DateTimeOffset), "date" },
            { typeof(Boolean), "boolean" }
        };

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static string GetReadableTypeName(Type type)
        //{
        //    if (type == null)
        //        return null;

        //    TypeNameAttribute modelNameAttribute = type.GetCustomAttribute<TypeNameAttribute>();
        //    if (modelNameAttribute != null && !String.IsNullOrEmpty(modelNameAttribute.Name))
        //    {
        //        return modelNameAttribute.Name;
        //    }

        //    string modelName = type.Name;
        //    if (type.IsGenericType)
        //    {
        //        // Format the generic type name to something like: GenericOfAgurment1AndArgument2
        //        Type genericType = type.GetGenericTypeDefinition();
        //        Type[] genericArguments = type.GetGenericArguments();
        //        string genericTypeName = genericType.Name;

        //        // Trim the generic parameter counts from the name
        //        genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
        //        string[] argumentTypeNames = genericArguments.Select(t => GetReadableTypeName(t)).ToArray();
        //        //modelName = String.Format(CultureInfo.InvariantCulture, "{0}Of{1}", genericTypeName, String.Join("And", argumentTypeNames));
        //        modelName = String.Format(CultureInfo.InvariantCulture, "{0}&lt;{1}&gt;", genericTypeName, String.Join(",", argumentTypeNames));
        //    }

        //    return modelName;
        //}

    }
}
