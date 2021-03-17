using Educ8IT.AspNetCore.SimpleApi.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation
{
    /// <summary>
    /// 
    /// </summary>
    public static class DocumentationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GetReadableTypeName(Type type)
        {
            if (type == null)
                return null;

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
    }
}
