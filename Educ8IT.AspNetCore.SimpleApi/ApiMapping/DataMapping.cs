using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ApiMapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataMapping
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="simpleValue"></param>
        ///// <param name="destinationType"></param>
        ///// <returns></returns>
        //[Obsolete()]
        //public static object ConvertParameter(string simpleValue, Type destinationType)
        //{
        //    if (destinationType == typeof(Guid))
        //        return simpleValue.ToGuid();

        //    else if (destinationType == typeof(Int64))
        //        return simpleValue.ToInt64(0);
        //    else if (destinationType == typeof(Int32))
        //        return simpleValue.ToInt32(0);

        //    else if (destinationType == typeof(Decimal))
        //        return simpleValue.ToDecimal(0M);

        //    else if (destinationType == typeof(Boolean))
        //        return simpleValue.ToBool(false);

        //    else if (destinationType == typeof(String))
        //        return simpleValue;

        //    else if (destinationType.BaseType == typeof(Enum))
        //        return Enum.Parse(destinationType, simpleValue);

        //    else return null;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nameValueCollection"></param>
        ///// <param name="parameterItem"></param>
        ///// <returns></returns>
        //[Obsolete("See GetInstanceFromNVC in ApiParameterItem")]
        //public static object ConvertToType(this NameValueCollection nameValueCollection, ApiParameterItem parameterItem)
        //{
        //    try
        //    {

        //        object __instance = Activator.CreateInstance(parameterItem.Type);

        //        //var __properties = parameterItem.Type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        //        if (parameterItem.SubProperties != null)
        //        {
        //            foreach (var __property in parameterItem.SubProperties)
        //            {
        //                var __keyMatch = nameValueCollection.AllKeys
        //                    .Where(k => k.ToLowerInvariant() == (parameterItem.Alias ?? __property.Name).ToLowerInvariant()).FirstOrDefault();

        //                __keyMatch = __keyMatch ?? nameValueCollection.AllKeys
        //                    .Where(k => k.ToLowerInvariant() == (__property.Alias ?? __property.Name).ToLowerInvariant()).FirstOrDefault();

        //                if (__keyMatch != null)
        //                    __property.PropertyInfo.SetValue(__instance, nameValueCollection[__keyMatch]);
        //            }
        //        }

        //        return __instance;
        //    }
        //    catch { }

        //    return null;
        //}
    }
}
