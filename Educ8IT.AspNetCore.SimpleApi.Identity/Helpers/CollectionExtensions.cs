using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
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
    }
}
