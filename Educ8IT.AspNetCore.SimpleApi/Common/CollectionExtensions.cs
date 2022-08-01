using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
    }
}
