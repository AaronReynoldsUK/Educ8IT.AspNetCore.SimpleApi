using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// otpauth://{type}/{label}:{account}?query...
    /// </summary>
    public class OtpUri : MfaUri
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_SCHEME_OTP = "otpauth";

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
        public const string MFA_PARAM_TYPE = "type";

        /// <summary>
        /// 
        /// </summary>
        public static string[] TOTP_FIELDS =
        {

        };

        /// <summary>
        /// 
        /// </summary>
        public static string[] HOTP_FIELDS =
        {

        };

        #endregion

        #region Patterns

        private static readonly string URL_ENCODED_TEXT = $"A-Za-z0-9\\%\\.\\-_";

        private static readonly string PATTERN_SCHEME_NAME = $"(?<scheme_name>{MFA_SCHEME_OTP})";
        private static readonly string PATTERN_SCHEME = $"(?<scheme>{PATTERN_SCHEME_NAME}://)";
        private static readonly string PATTERN_TYPE = $"(?<type>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_LABEL = $"(?<label>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_ACCOUNT = $"(?<account>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_PARAM_KEY = $"(?<key>[{URL_ENCODED_TEXT}]{{1,255}})";
        private static readonly string PATTERN_PARAM_VALUE = $"(?<value>[{URL_ENCODED_TEXT}]{{0,255}})";

        private static readonly string PATTERN_PARAM_SET = $"{PATTERN_PARAM_KEY}={PATTERN_PARAM_VALUE}";
        private static readonly string PATTERN_QUERY = $"(?:\\?{PATTERN_PARAM_SET}(?:\\&{PATTERN_PARAM_SET})*)?";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PATTERN_OTP_AUTH = $"^{PATTERN_SCHEME}{PATTERN_TYPE}/{PATTERN_LABEL}(?:\\:{PATTERN_ACCOUNT})?{PATTERN_QUERY}$";

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get
            {
                return GetQueryValue(MFA_PARAM_TYPE, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Counter
        {
            get
            {
                var __counter = GetQueryValue(MFA_PARAM_COUNTER, null);
                if (__counter != null)
                {
                    if (long.TryParse(__counter, out long __value))
                        return __value;
                }

                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string Code
        {
            get; set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaUri"></param>
        public OtpUri(MfaUri mfaUri) : base(mfaUri.OriginalUri)
        {
            if (String.IsNullOrEmpty(Secret))
                return;

            // Get Key + Generate OTC
            byte[] __secretKey = Identity.OtpNet.Base32Encoding.ToBytes(Secret);

            if (Type == OTP_TYPE_TOTP)
            {
                var __totp = new Identity.OtpNet.Totp(__secretKey);
                this.Code = __totp.ComputeTotp();
            }
            else if (Type == OTP_TYPE_HOTP)
            {
                var __hotp = new Identity.OtpNet.Hotp(__secretKey);
                this.Code = __hotp.ComputeHOTP(Counter);
            }
        }

        #endregion

        #region Public Member Functions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Secret))
                return null;

            // Build this string
            StringBuilder sb = new StringBuilder();
            List<string> __paramSet = new List<string>();

            var __secretBytes = Identity.OtpNet.Base32Encoding.ToBytes(Secret);
            string[] __fieldList = null;

            sb.Append($"{MFA_SCHEME_OTP}://");
            sb.Append($"{Type}/");
            sb.Append($"{WebUtility.UrlEncode(Label)}");

            if (!String.IsNullOrEmpty(Account))
            {
                sb.Append($":{WebUtility.UrlEncode(Account)}");
            }

            if (Type == OTP_TYPE_TOTP)
            {
                var __totp = new Identity.OtpNet.Totp(__secretBytes);
                this.Code = __totp.ComputeTotp();

                __fieldList = TOTP_FIELDS;
            }
            else if (Type == OTP_TYPE_HOTP)
            {
                var __hotp = new Identity.OtpNet.Hotp(__secretBytes);
                this.Code = __hotp.ComputeHOTP(Counter);

                __fieldList = HOTP_FIELDS;
            }
            else
            {
                throw new Exception("Unsupported OTP");
            }

            foreach (var __key in __fieldList)
            {
                if (String.IsNullOrEmpty(__key))
                    continue;

                // will work when we shift this all into Authentication/Mfa/UriSchemes
                var __value = QueryParams.GetValue(__key);

                if (!String.IsNullOrEmpty(__value))
                {
                    __paramSet.Add($"{WebUtility.UrlEncode(__key)}={WebUtility.UrlEncode(__value)}");
                }
            }

            if (__paramSet != null && __paramSet.Count > 0)
            {
                sb.Append("?");
                sb.Append(String.Join("&", __paramSet.ToArray()));
            }

            return base.ToString();
        }

        #endregion
    }
}
