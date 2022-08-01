using System;
using System.Linq;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Concatenation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayOne"></param>
        /// <param name="arrayTwo"></param>
        /// <param name="allowDuplicates"></param>
        /// <returns></returns>
        public static T[] Combine<T>(T[] arrayOne, T[] arrayTwo, bool allowDuplicates)
        {
            List<T> __tmp = new List<T>();

            if (arrayOne != null)
                __tmp.AddRange(arrayOne);

            if (arrayTwo != null)
                __tmp.AddRange(arrayTwo);

            return allowDuplicates
                ? __tmp.ToArray()
                : __tmp.Distinct().ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayOne"></param>
        /// <param name="singleValue"></param>
        /// <param name="allowDuplicates"></param>
        /// <returns></returns>
        public static T[] Combine<T>(T[] arrayOne, T singleValue, bool allowDuplicates)
        {
            List<T> __tmp = new List<T>();

            if (arrayOne != null)
                __tmp.AddRange(arrayOne);

            if (singleValue != null)
                __tmp.Add(singleValue);

            return allowDuplicates
                ? __tmp.ToArray()
                : __tmp.Distinct().ToArray();
        }

        //public static IDictionary<K,V> Combine<K,V>(IDictionary<K, V> dictionaryOne, IDictionary<K, V> dictionaryTwo)
        //{
        //    IDictionary<K,V> __tmp = new Dictionary<K,V>();

        //    if (dictionaryOne != null)
        //        __tmp.Concat(dictionaryOne);

        //    if (dictionaryTwo != null)
        //    {
        //        dictionaryTwo.Distinct()
        //    }
        //}
    }
}
