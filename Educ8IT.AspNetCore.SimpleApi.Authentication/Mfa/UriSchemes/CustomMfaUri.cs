using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa.UriSchemes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CustomMfaUri : Uri, ICustomMfaUri
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        public CustomMfaUri(string uriString) : base(uriString) { }

        #endregion

        #region Constants and Static Properties

        /// <inheritdoc/>
        public static string PATTERN_CUSTOM_URL { get; }

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_CUSTOM_URL
        {
            get
            {
                return _REGEX_CUSTOM_URL ??= new Regex(PATTERN_CUSTOM_URL,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            }
        }

        #endregion

        #region Fields

        private static Regex _REGEX_CUSTOM_URL = null;
        private NameValueCollection _queryParams = null;
        
        #endregion

        #region Instance Properties

        /// <inheritdoc/>
        public abstract string MFA_SCHEME { get; }

        /// <inheritdoc/>
        public abstract string[] ALLOWED_QUERY_KEYS { get; }

        /// <inheritdoc/>
        public bool IsSchemeMatch
        {
            get
            {
                if (MFA_SCHEME == null)
                    return false;


                return MFA_SCHEME == this.Scheme;
            }
        }

        /// <inheritdoc/>
        public NameValueCollection QueryParameters
        {
            get { return _queryParams ??= GetQueryParameters(); }
        }

        #endregion

        #region Private Member Functions

        private NameValueCollection GetQueryParameters()
        {
            if (String.IsNullOrEmpty(this.Query))
                return null;

            var __nameValueCollection = new NameValueCollection();

            var __querySets = this.Query.Split("&");
            foreach (var __set in __querySets)
            {
                if (String.IsNullOrEmpty(__set))
                    continue;

                var __parts = __set.Split("=");
                if (__parts.Length == 0)
                    continue;

                if (String.IsNullOrEmpty(__parts[0]))
                    continue;

                var __key = __parts[0];
                if (__nameValueCollection.HasKey(__key))
                    continue;

                var __value = (__parts.Length == 2)
                    ? __parts[1]
                    : null;

                __nameValueCollection.Set(__key, __value);
            }
            return __nameValueCollection;
        }

        #endregion

        #region Public Member Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetQueryValue(string keyName, string defaultValue = null)
        {
            var __queryParams = QueryParameters;

            if (String.IsNullOrEmpty(keyName))
                throw new ArgumentNullException(nameof(keyName));

            if (__queryParams == null || __queryParams.Count == 0 || !__queryParams.HasKey(keyName))
                return defaultValue;

            return __queryParams.Get(keyName) ?? defaultValue;
        }

        #endregion

        #region Static Functions

        /// <inheritdoc/>
        public static bool TryCreateFromUri<T>(System.Uri uri, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        /// <inheritdoc/>
        public static bool TryCreateFromUriString<T>(string uriString, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        /// <inheritdoc/>
        public static bool TryCreateUriFromCustomUriString(string customUriString, out System.Uri uri)
        {
            uri = default(System.Uri);
            return false;
        }

        /// <inheritdoc/>
        public static bool TryCreateFromCustomUriString<T>(string customUriString, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static bool IsValid<T>(string uriString) where T : struct, ICustomMfaUri
        {
            if (uriString == null)
                return false;

            return Regex.IsMatch(uriString, PATTERN_CUSTOM_URL, 
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="uriString"></param>
        ///// <param name="customUri"></param>
        ///// <returns></returns>
        //public static bool _TryParse<T>(string uriString, out T customUri) where T : struct, ICustomMfaUri
        //{
        //    customUri = default(T);

        //    if (uriString == null)
        //        return false;

        //    try
        //    {
        //        customUri = (T)Activator.CreateInstance(typeof(T), uriString);
        //    }
        //    catch { }

        //    return !customUri.Equals(null);
        //}

        //public static bool TryParse(string customUriString, out System.Uri uri)
        //{
        //    uri = default;

        //    return false;
        //}

        #endregion

        #region Other Member Functions

        /// <inheritdoc/>
        public virtual string GetAsNativeUriString()
        {
            return this.ToString();
        }

        /// <inheritdoc/>
        public string GetDataStoreUriString()
        {
            // mfa://type/?query

            StringBuilder sb = new StringBuilder();

            sb.Append($"mfa://");
            sb.Append($"{WebUtility.UrlEncode(MFA_SCHEME)}/");

            if (QueryParameters != null && QueryParameters.Count > 0)
            {
                List<string> __paramSet = new List<string>();
                foreach (var key in QueryParameters.AllKeys)
                {
                    if (String.IsNullOrEmpty(key))
                        continue;

                    if (ALLOWED_QUERY_KEYS != null && !ALLOWED_QUERY_KEYS.Contains(key))
                        continue;

                    __paramSet.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(QueryParameters[key])}");
                }
                if (__paramSet != null && __paramSet.Count > 0)
                {
                    sb.Append("?");
                    sb.Append(String.Join("&", __paramSet.ToArray()));
                }
            }

            return sb.ToString();
        }

        /// <inheritdoc/>
        public virtual string GetInitUriString() { return null; }

        /// <inheritdoc/>
        public virtual string GetPromptUriString() { return null; }

        /// <inheritdoc/>
        public virtual string GetOutOfBandUriString() { return null; }

        #endregion
    }
}
