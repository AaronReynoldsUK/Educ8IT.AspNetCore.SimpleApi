using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// This is format we use to store MFA configuration.
    /// We can map from this to individual Uri schemes for sending.
    /// Format = mfa://{method}/{label}:{account}?params
    /// </summary>
    public class MfaUri
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public static string[] KNOWN_MFA_SCHEMES = 
        {
            "totp", "hotp",
            "mailto",
            "sms",
            "tel"
        };

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_SECRET = "secret";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_ISSUER = "issuer";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_ALGORITHM = "algorithm";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_DIGITS = "digits";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_PERIOD = "period";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_COUNTER = "counter";

        #endregion

        #region Patterns

        private const string URL_SCHEME_MFA = "mfa";

        private static readonly string PATTERN_MFA_SCHEME_NAME = $"(?<scheme_name>{URL_SCHEME_MFA})";
        private static readonly string PATTERN_MFA_SCHEME = $"(?<scheme>{PATTERN_MFA_SCHEME_NAME}://)";
        private static readonly string PATTERN_MFA_METHOD = $"(?<method>{String.Join("|", KNOWN_MFA_SCHEMES)})";
        private static readonly string PATTERN_MFA_LABEL = $"(?<label>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";
        private static readonly string PATTERN_ACCOUNT = $"(?<account>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PATTERN_MFA_URL =
            $"^{PATTERN_MFA_SCHEME}/{PATTERN_MFA_METHOD}/{PATTERN_MFA_LABEL}(?:\\:{PATTERN_ACCOUNT})?{Patterns.PATTERN_URL_QUERY}?$";

        #endregion

        #region Regular Expressions

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_MFA_URL = new Regex(
                PATTERN_MFA_URL,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public virtual string URL_SCHEME_NAME { get; } = URL_SCHEME_MFA;

        /// <summary>
        /// 
        /// </summary>
        public string OriginalUri { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection QueryParams { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Secret
        {
            get
            {
                return GetQueryValue(MFA_PARAM_SECRET, null);
            }
            set
            {
                if (QueryParams == null)
                    QueryParams = new NameValueCollection();

                QueryParams[MFA_PARAM_SECRET] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        public MfaUri(string uriString)
        {
            OriginalUri = uriString;

            Parse();
        }

        #endregion

        #region Private Member Functions

        private void Reset()
        {
            SchemeName = String.Empty;
            Method = String.Empty;
            Label = String.Empty;
            Account = String.Empty;
            QueryParams = new NameValueCollection();
        }

        private void Parse()
        {
            MatchCollection __mc = null;
            List<string> __keys = new List<string>();
            List<string> __values = new List<string>();

            Reset();

            if (OriginalUri == null)
                return;

            if (REGEX_MFA_URL.IsMatch(OriginalUri))
            {
                __mc = REGEX_MFA_URL.Matches(OriginalUri);

                foreach (Match m in __mc)
                {
                    if (!m.Success)
                        continue;

                    foreach (Group g in m.Groups)
                    {
                        switch (g.Name)
                        {
                            case "scheme_name":
                                SchemeName = g.Value;
                                break;

                            case "method":
                                Method = WebUtility.UrlDecode(g.Value);
                                break;

                            case "label":
                                Label = WebUtility.UrlDecode(g.Value);
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
                        this.QueryParams.Add(__keys[x], __values[x]);
                    }
                }
            }
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
        /// <param name="mfaMethod"></param>
        /// <returns></returns>
        public EMfaMethod Convert(string mfaMethod)
        {
            EMfaMethod method = EMfaMethod.None;

            switch (mfaMethod)
            {
                case "totp":
                case "hotp":
                    method = EMfaMethod.TOTP;
                    break;

                case "mailto":
                    method = EMfaMethod.Email;
                    break;

                case "sms":
                    method = EMfaMethod.SMS;
                    break;

                case "tel":
                    method = EMfaMethod.Telephone;
                    break;
            }

            return method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var __builder = new MfaUriBuilder(this);

            return __builder.ToString();
        }

        #endregion

        #region Static Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="mfaUri"></param>
        /// <returns></returns>
        public static bool TryParse(string uriString, out MfaUri mfaUri)
        {
            mfaUri = null;

            if (uriString == null)
                return false;

            try
            {
                mfaUri = new MfaUri(uriString);
            }
            catch { }

            return mfaUri != null;
        }

        #endregion
    }
}
