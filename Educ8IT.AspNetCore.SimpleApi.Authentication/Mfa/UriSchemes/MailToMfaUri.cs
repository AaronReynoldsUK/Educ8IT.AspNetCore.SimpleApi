using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa.UriSchemes
{
    // OK, don't super-class the Uri + don't need Interfaces

    /*
     * what we need:
    * - config is stored in a standard uri with scheme = mfa://
    * - validate mfa scheme
    * - convert mfa-scheme into totp url/object
    * - convert mfa-scheme into mailto info for confirmation email
    * - convert ... for sms or tel
    *
    * also:
    * - validate totp, mailto, sms, tel, ... etc scheme
    * - convert scheme into object (e.g. mailto:... into object with mailto properties)
    * 
    * class for MFA sheme
    * class for e.g. mailto
    * 
    * extensions (e.g.):
    * - Uri.ToMailToPromptUri()
    * - Uri.ToTotpAppUri()
    * 
    */

    public static class MailToMfaUriConverter
    {
        public Convert ToUri(MailToMfaUri eg);

        public Convert ToMailTo(Uri uri)
    }


    public class MailToToUri
    {

    }

    public static class MailToMfaUriExtensions
    {
        public static string ToOutOfBandMailTo(this Uri uri);

        public static Uri FromMailTo(this string mailToUri);

        public static bool IsValidConfig(Identity.EMfaMethod method, ...);

        public static string ToMailTo(this Uri uri);

        public static MailToMfaUri FromUri(this Uri uri)
        { }

        public static MailToMfaUri FromUriString(this string uriString)
        { }

        public static bool TryParse(string uriString, out )
    }

    /// <summary>
    /// 
    /// </summary>
    public class MailToMfaUri : CustomMfaUri
    {
        #region Constructors

        private MailToMfaUri(string uriString) : base(uriString) { }

        public MailToMfaUri(string customUriString):
            base()

        #endregion

        #region Constants and Static Properties

        /// <summary>
        /// 
        /// </summary>
        public const string MAILTO_PARAM_TO = "to";

        /// <summary>
        /// 
        /// </summary>
        public const string MAILTO_PARAM_CC = "cc";

        /// <summary>
        /// 
        /// </summary>
        public const string MAILTO_PARAM_SUBJECT = "subject";

        /// <summary>
        /// 
        /// </summary>
        public const string MAILTO_PARAM_BODY = "body";

        /// <inheritdoc/>
        public new static string PATTERN_CUSTOM_URL
        {
            get { return _PATTERN_CUSTOM_URL; }
        }

        #endregion

        #region Fields

        private const string _MFA_SCHEME = "mailto";

        private static string[] _ALLOWED_QUERY_KEYS =
        {
            MAILTO_PARAM_CC,
            MAILTO_PARAM_SUBJECT,
            MAILTO_PARAM_BODY
        };

        private static readonly string PATTERN_SCHEME = $"(?<scheme>{_MFA_SCHEME})";

        private static string _PATTERN_CUSTOM_URL = $"^{PATTERN_SCHEME}:({Patterns.PATTERN_EMAIL_ADDRESS})?({Patterns.PATTERN_URL_QUERY})?$";

        #endregion

        #region Instance Properties

        /// <inheritdoc/>
        public override string MFA_SCHEME
        {
            get { return _MFA_SCHEME; }
        }

        /// <inheritdoc/>
        public override string[] ALLOWED_QUERY_KEYS
        {
            get { return _ALLOWED_QUERY_KEYS; }
        }

        #endregion

        #region Private Static Functions

        private static Uri UriFromCustomUriString(string customUriString)
        {
            UriBuilder uriBuilder = new UriBuilder();

            if (!REGEX_CUSTOM_URL.IsMatch(customUriString))
                throw new ArgumentException("Invalid Uri String");

            var __mc = REGEX_CUSTOM_URL.Matches(customUriString);
            NameValueCollection nvcQuery = new NameValueCollection();
            List<string> __keys = new List<string>();
            List<string> __values = new List<string>();

            foreach (Match m in __mc)
            {
                if (!m.Success)
                    continue;

                foreach (Group g in m.Groups)
                {
                    switch (g.Name)
                    {
                        case "scheme":
                            uriBuilder.Scheme = g.Value;
                            break;

                        case "email":
                            nvcQuery.Add(MAILTO_PARAM_TO, WebUtility.UrlDecode(g.Value));
                            break;

                        case "key":
                            foreach (var __capture in g.Captures)
                            {
                                if (__capture == null)
                                    continue;

                                __keys.Add(WebUtility.UrlDecode(__capture.ToString()));
                            }
                            break;

                        case "value":
                            foreach (var __capture in g.Captures)
                            {
                                __values.Add(WebUtility.UrlDecode(__capture?.ToString() ?? String.Empty));
                            }
                            break;
                    }
                }
            }

            if (__keys.Count == __values.Count)
            {
                for (int x = 0; x < __keys.Count; x++)
                {
                    if (nvcQuery.HasKey(__keys[x]))
                        continue;

                    nvcQuery.Add(__keys[x], __values[x]);
                }
            }

            uriBuilder.Query = nvcQuery.ToQueryString();

            return uriBuilder.Uri;
        }

        #endregion

        #region Static Functions

        public static bool TryCreateFromUri<T>(Uri uri, out T customUri) where T : struct, ICustomMfaUri
        {
            
            customUri = new ICustomMfaUri(T)(new MailToMfaUri(uri.ToString()));
            return false;
        }

        public static new bool TryCreateFromUriString<T>(string uriString, out T customUri) where T : struct, ICustomMfaUri
        {
            
            customUri = new MailToMfaUri(uriString);

            return customUri != null;
        }

        public static bool TryCreateUriFromCustomUriString(string customUriString, out Uri uri)
        {
            try
            {
                uri = UriFromCustomUriString(customUriString);
            }
            catch
            {
                uri = null;
            }
            return uri != null;
        }

        public static bool TryCreateFromCustomUriString(string customUriString, out MailToMfaUri customUri)
        {
            customUri = null;
            if (TryCreateUriFromCustomUriString(customUriString, out Uri uri))
            {
                customUri = new MailToMfaUri(uri.ToString());
            }
            return customUri != null;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="customUriString"></param>
        ///// <returns></returns>
        //public static string UriStringFromCustomUriString(string customUriString)
        //{
        //    return UriFromCustomUriString(customUriString).ToString();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="customUriString"></param>
        ///// <returns></returns>
        //public static string FromCustomUriString(string customUriString)
        //{
        //    return new  UriFromCustomUriString(customUriString).ToString();
        //}

        ///// <summary>
        ///// Parse a mailto: Uri
        ///// </summary>
        ///// <param name="mailtoUri"></param>
        ///// <param name="uri"></param>
        ///// <returns></returns>
        //public new static bool TryParse(string mailtoUri, out Uri uri)
        //{
        //    try
        //    {
        //        uri = MailToMfaUri.UriFromCustomUriString(mailtoUri);
        //    }
        //    catch
        //    {
        //        uri = null;
        //    }

        //    return uri != null;
        //}

        ///// <summary>
        ///// Parse a mailto: Uri
        ///// </summary>
        ///// <param name="mailtoUri"></param>
        ///// <param name="uri"></param>
        ///// <returns></returns>
        //public new static bool TryParse(string mailtoUri, out Uri uri)
        //{
        //    try
        //    {
        //        uri = MailToMfaUri.UriFromCustomUriString(mailtoUri);
        //    }
        //    catch
        //    {
        //        uri = null;
        //    }

        //    return uri != null;
        //}

        #endregion

        #region Other Member Functions

        /// <summary>
        /// Get as mailto: Uri
        /// </summary>
        /// <returns></returns>
        public override string GetAsNativeUriString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"mailto:");

            var __to = GetQueryValue(MAILTO_PARAM_TO);

            if (!String.IsNullOrEmpty(__to))
                sb.Append(__to);

            return sb.ToString();
        }

        #endregion
    }
}
