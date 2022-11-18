using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomUri
    {
        #region Constants and Statics

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public abstract string[] URL_SCHEMES_HANDLED { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string PATTERN_CUSTOM_URL { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract Regex REGEX_CUSTOM_URL { get; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalUri { get; }

        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection QueryParams { get; }

        #endregion

        #region Member Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetQueryValue(string keyName, string defaultValue = null)
        {
            if (QueryParams != null && QueryParams.Count > 0 && QueryParams.HasKeys())
            {
                if (QueryParams.AllKeys.Contains(keyName))
                {
                    return QueryParams[keyName];
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        void Reset();

        /// <summary>
        /// 
        /// </summary>
        void Parse(string uriString);

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uriString"></param>
        /// <param name="customUri"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string uriString, out T customUri) where T : struct, ICustomUri
        {
            customUri = default(T);

            if (uriString == null)
                return false;

            try
            {
                customUri = (T)Activator.CreateInstance(typeof(T), uriString);
            }
            catch { }

            return !customUri.Equals(null);
        }
    }
}
