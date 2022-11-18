using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// Format = sms:{recipient}?prompt={prompt}&code={code}
    /// </summary>
    public class SmsUri : MfaUri
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_SCHEME_SMS = "sms";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_RECIPIENT = "recipient";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_PROMPT = "prompt";

        /// <summary>
        /// 
        /// </summary>
        public const string MFA_PARAM_CODE = "code";

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string URL_SCHEME_NAME { get => MFA_SCHEME_SMS; }

        /// <summary>
        /// 
        /// </summary>
        public string TelephoneNumber
        {
            get
            {
                return GetQueryValue(MFA_PARAM_RECIPIENT, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Prompt
        {
            get
            {
                return GetQueryValue(MFA_PARAM_PROMPT, null);
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
        public SmsUri(MfaUri mfaUri) : base(mfaUri.OriginalUri)
        {
            if (String.IsNullOrEmpty(Secret))
                return;

            // Get Key + Generate OTC
            byte[] __secretKey = Identity.OtpNet.Base32Encoding.ToBytes(Secret);
            var __totp = new Identity.OtpNet.Totp(__secretKey);
            this.Code = __totp.ComputeTotp();
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

            sb.Append($"{URL_SCHEME_NAME}:");
            sb.Append($"{TelephoneNumber}");

            List<string> __paramSet = new List<string>();

            if (!String.IsNullOrEmpty(Prompt))
                __paramSet.Add($"{WebUtility.UrlEncode(MFA_PARAM_PROMPT)}={WebUtility.UrlEncode(Prompt)}");

            if (!String.IsNullOrEmpty(Code))
                __paramSet.Add($"{WebUtility.UrlEncode(MFA_PARAM_CODE)}={WebUtility.UrlEncode(Code)}");

            if (__paramSet != null && __paramSet.Count > 0)
            {
                sb.Append("?");
                sb.Append(String.Join("&", __paramSet.ToArray()));
            }

            return base.ToString();
        }

        #endregion

        #region Static Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="mailToUri"></param>
        /// <returns></returns>
        public static bool TryParse(string uriString, out MailToUri mailToUri)
        {
            mailToUri = null;

            if (uriString == null)
                return false;

            try
            {
                mailToUri = new MailToUri(uriString);
            }
            catch { }

            return mailToUri != null;
        }

        #endregion
    }
}
