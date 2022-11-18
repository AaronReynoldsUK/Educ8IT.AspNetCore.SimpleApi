using Educ8IT.AspNetCore.SimpleApi.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// Base type for MFA method.
    /// read/write MFA Uri for this provider
    /// </summary>
    public abstract class BaseMfaProviderUri : IBaseMfaProviderUri
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public BaseMfaProviderUri(MfaDataUri uri)
        {
            mfaDataUri = new MfaDataUri(uri?.AbsoluteUri);
        }

        /// <summary>
        /// Parses the customUriString into an MfaDataUri object
        /// </summary>
        /// <param name="customUriString"></param>
        public BaseMfaProviderUri(string customUriString)
        {
            if (!Uri.TryCreate(customUriString, UriKind.Absolute, out Uri uri))
                throw new ArgumentException("Invalid Custom Uri", nameof(customUriString));

            if (uri.Scheme != SCHEME_NAME)
                throw new ArgumentException("Invalid Custom Uri Scheme", nameof(customUriString));

            mfaDataUri = ParseIntoMfaDataUri(uri);
        }

        #endregion

        #region Constants

        //private const string _SCHEME_NAME = "mfa";

        #endregion

        #region Fields

        private MfaDataUri mfaDataUri = null;

        private NameValueCollection _queryParams = null;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MfaDataUri DataUri
        {
            get { return this.mfaDataUri; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public Uri CustomUri
        //{
        //    get { return this.customUri; }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public Uri Uri
        //{
        //    get
        //    {
        //        return DataUri ?? CustomUri;
        //    }
        //}

        #endregion

        #region Abstract Properties

        /// <inheritdoc/>
        public abstract string SCHEME_NAME { get; }

        /// <inheritdoc/>
        public abstract string PATTERN_MFA_URL { get; }

        /// <inheritdoc/>
        public abstract System.Text.RegularExpressions.Regex REGEX_MFA_URL { get; }


        /// <inheritdoc/>
        public abstract bool IsValid { get; }

        /// <inheritdoc/>
        public NameValueCollection QueryParams
        {
            get { return _queryParams ??= GetQueryParameters(this.DataUri?.Query); }
        }

        #endregion

        #region Member Functions

        /// <inheritdoc/>
        public NameValueCollection GetQueryParameters(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
                return null;

            var __nameValueCollection = new NameValueCollection();

            var __querySets = queryString.Split("&");
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public abstract MfaDataUri ParseIntoMfaDataUri(System.Uri customUri);


        /// <inheritdoc/>
        public virtual string GetInitUriString() { return null; }

        /// <inheritdoc/>
        public virtual string GetPromptUriString() { return null; }

        /// <inheritdoc/>
        public virtual string GetOutOfBandUriString() { return null; }

        #endregion
    }
}
