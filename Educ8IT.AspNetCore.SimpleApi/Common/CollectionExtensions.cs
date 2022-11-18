using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AppendToEntry(this IHeaderDictionary headers, string key, string value)
        {
            if (headers.TryGetValue(key, out var entry))
            {
                headers[key] = Concatenation.Combine(entry.ToArray(), value, false);
            }
            else
            {
                headers[key] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="entry"></param>
        public static void AddIfNotExist<T>(this IList<T> list, T entry)
        {
            if (list == null || entry == null)
                return;

            if (list.Contains(entry))
                return;

            list.Add(entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasKey(this NameValueCollection nameValueCollection, string key)
        {
            return (nameValueCollection.AllKeys?.Contains(key) ?? false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection nameValueCollection, string key, string defaultValue = null)
        {
            return (nameValueCollection.AllKeys?.Contains(key) ?? false)
                ? nameValueCollection[key] ?? defaultValue
                : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
                throw new ArgumentNullException(nameof(nameValueCollection));

            List<string> querySets = new List<string>();
            foreach (var key in nameValueCollection.AllKeys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                querySets.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(nameValueCollection[key])}");
            }

            return (querySets != null && querySets.Count > 0)
                ? $"?{string.Join("&", querySets)}"
                : String.Empty;
        }
    }
}
