using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication
{
    public class OtpUriBuilder
    {
        #region Properties

        public string SchemeName { get; set; } = OtpUri.URI_SCHEME_OTPAUTH;

        public string Type { get; set; }

        public string Label { get; set; }

        public string Account { get; set; }

        public NameValueCollection QueryParams { get; private set; }

        #endregion

        private void Reset()
        {
            SchemeName = String.Empty;
            Type = String.Empty;
            Label = String.Empty;
            Account = String.Empty;
            QueryParams = new NameValueCollection();
        }

        public OtpUriBuilder SetSchemeName(string schemeName)
        {
            SchemeName = schemeName;
            return this;
        }

        public OtpUriBuilder SetType(string type)
        {
            Type = type;
            return this;
        }

        public OtpUriBuilder SetLabel(string label)
        {
            Label = label;
            return this;
        }

        public OtpUriBuilder SetAccount(string account)
        {
            Account = account;
            return this;
        }

        public OtpUriBuilder AddQuerySet(string key, string value)
        {
            if (QueryParams == null)
                QueryParams = new NameValueCollection();

            QueryParams.Add(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OtpUri Build()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{SchemeName}://");
            sb.Append($"{WebUtility.UrlEncode(Type)}/");
            sb.Append($"{WebUtility.UrlEncode(Label)}");

            if (!String.IsNullOrEmpty(Account))
            {
                sb.Append($":{WebUtility.UrlEncode(Account)}");
            }

            if (QueryParams != null && QueryParams.Count > 0)
            {
                List<string> __paramSet = new List<string>();
                foreach (var key in QueryParams.AllKeys)
                {
                    __paramSet.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(QueryParams[key])}");
                }
                if (__paramSet != null && __paramSet.Count > 0)
                {
                    sb.Append("?");
                    sb.Append(String.Join("&", __paramSet.ToArray()));
                }
            }

            return new OtpUri(sb.ToString());
        }
    }

    //public static class OtpUriBuilderExtensions
    //{
    //    public static OtpUriBuilder SetSchemeName(this OtpUriBuilder otpUriBuilder, string schemeName)
    //    {
    //        otpUriBuilder.SchemeName = schemeName;

    //        return otpUriBuilder;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public class OtpUri
    {
        /// <summary>
        /// 
        /// </summary>
        public const string URI_SCHEME_OTPAUTH = "otpauth";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_TYPE_TOTP = "totp";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_TYPE_HOTP = "hotp";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_PARAM_SECRET = "secret";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_PARAM_ISSUER = "issuer";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_PARAM_ALGORITHM = "algorithm";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_PARAM_DIGITS = "digits";

        /// <summary>
        /// 
        /// </summary>
        public const string OTP_PARAM_PERIOD = "period";

        private static readonly string URL_ENCODED_TEXT = $"A-Za-z0-9\\%\\.\\-_";

        private static readonly string PATTERN_SCHEME_NAME = $"(?<scheme_name>{URI_SCHEME_OTPAUTH})";
        private static readonly string PATTERN_SCHEME = $"(?<scheme>{PATTERN_SCHEME_NAME}://)";
        private static readonly string PATTERN_TYPE = $"(?<type>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_LABEL = $"(?<label>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_ACCOUNT = $"(?<account>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_PARAM_KEY = $"(?<key>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_PARAM_VALUE = $"(?<value>[{URL_ENCODED_TEXT}]{{0,255}})";

        private static readonly string PATTERN_PARAM_SET = $"{PATTERN_PARAM_KEY}={PATTERN_PARAM_VALUE}";
        private static readonly string PATTERN_QUERY = $"(?:\\?{PATTERN_PARAM_SET}(?:\\&{PATTERN_PARAM_SET})*)?";
        // ^(?<scheme>otpauth://)(?<type>[A-Za-z0-9\%\.\-_]{1,255})/(?<label>[A-Za-z0-9\%\.\-_]{1,255})(?:\:(?<account>[A-Za-z0-9\%\.\-_]{1,255}))?\?((?<key>[A-Za-z0-9\%\.\-_]{1,255})=(?<value>[A-Za-z0-9\%\.\-_]{0,255})(?:\&(?<key>[A-Za-z0-9\%\.\-_]{1,255})=(?<value>[A-Za-z0-9\%\.\-_]{0,255}))*)?
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly string PATTERN_OTP_AUTH = $"^{PATTERN_SCHEME}{PATTERN_TYPE}/{PATTERN_LABEL}(?:\\:{PATTERN_ACCOUNT})?{PATTERN_QUERY}$";

        private static System.Text.RegularExpressions.Regex REGEX_OTP_AUTH = new System.Text.RegularExpressions.Regex(PATTERN_OTP_AUTH, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // Params = google_authenticator|{step}|{mode}|{size}|{secret}
        // Uri = otpauth://totp/SimpleApi:email@email.com?secret={secret}&issuer={provider}&algorithm=SHA1&digits=6&period=30

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        public OtpUri(string uriString)
        {
            OriginalUri = uriString;

            Parse();
        }

        #region Properties

        public string OriginalUri { get; private set; }

        public string SchemeName { get; private set; }

        public string Type { get; private set; }

        public string Label { get; private set; }

        public string Account { get; private set; }

        public NameValueCollection QueryParams { get; private set; }

        private List<string> _Keys;
        private List<string> _Values;

        #endregion
        //public bool IsValid

        private void Reset()
        {
            SchemeName = String.Empty;
            Type = String.Empty;
            Label = String.Empty;
            Account = String.Empty;
            QueryParams = new NameValueCollection();
            _Keys = new List<string>();
            _Values = new List<string>();
        }

        private void Parse()
        {
            Reset();

            // Also need to UrlDecode...

            if (OriginalUri == null)
                return;

            if (REGEX_OTP_AUTH.IsMatch(OriginalUri))
            {
                var __mc = REGEX_OTP_AUTH.Matches(OriginalUri);

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

                            case "type":
                                Type = WebUtility.UrlDecode(g.Value);
                                break;

                            case "label":
                                Label = WebUtility.UrlDecode(g.Value);
                                break;

                            case "account":
                                Account = WebUtility.UrlDecode(g.Value);
                                break;

                            case "key":
                                foreach (var __capture in g.Captures)
                                {
                                    if (__capture == null)
                                        continue;

                                    _Keys.Add(WebUtility.UrlDecode(__capture.ToString()));
                                }
                                break;

                            case "value":
                                foreach (var __capture in g.Captures)
                                {
                                    _Values.Add(WebUtility.UrlDecode(__capture?.ToString() ?? String.Empty));
                                }
                                break;
                        }
                    }
                }

                if (this._Keys.Count == this._Values.Count)
                {
                    for (int x=0; x< this._Keys.Count; x++)
                    {
                        this.QueryParams.Add(this._Keys[x], this._Values[x]);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="otpUri"></param>
        /// <returns></returns>
        public static bool TryParse(string uriString, out OtpUri otpUri)
        {
            otpUri = null;

            if (uriString == null)
                return false;

            try
            {
                otpUri = new OtpUri(uriString);
            }
            catch { }

            return otpUri != null;
        }

        public override string ToString()
        {
            return OriginalUri ?? base.ToString();
        }
    }
}
