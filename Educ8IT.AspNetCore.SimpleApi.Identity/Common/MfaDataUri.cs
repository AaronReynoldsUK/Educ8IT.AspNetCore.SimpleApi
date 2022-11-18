using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// used to store MFA data, provides validation and is of protocol/scheme type "mfa"
    /// </summary>
    public class MfaDataUri : Uri
    {
        #region Constructors

        /// <summary>
        /// Construct from Uri string
        /// </summary>
        /// <param name="uriString"></param>
        public MfaDataUri(string uriString) : base(uriString) { }

        #endregion

        #region Constants

        /// <summary>
        /// Rqeuired scheme name for this Uri
        /// </summary>
        public const string SCHEME_NAME = "mfa";

        #endregion

        #region Static Fields

        private static readonly string PATTERN_MFA_SCHEME_NAME = $"(?<scheme_name>{SCHEME_NAME})";
        private static readonly string PATTERN_MFA_SCHEME = $"(?<scheme>{PATTERN_MFA_SCHEME_NAME}://)";
        private static readonly string PATTERN_MFA_METHOD = $"(?<method>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";
        private static readonly string PATTERN_MFA_LABEL = $"(?<label>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";
        private static readonly string PATTERN_ACCOUNT = $"(?<account>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";

        private static readonly string _PATTERN_MFA_URL =
            $"^{PATTERN_MFA_SCHEME}/{PATTERN_MFA_METHOD}/{PATTERN_MFA_LABEL}(?:\\:{PATTERN_ACCOUNT})?{Patterns.PATTERN_URL_QUERY}?$";

        private static readonly Regex _REGEX_MFA_URL = new Regex(
                _PATTERN_MFA_URL,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string PATTERN_MFA_URL
        {
            get
            {
                return _PATTERN_MFA_URL;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Regex REGEX_MFA_URL
        {
            get
            {
                return _REGEX_MFA_URL;
            }
        }

        /// <summary>
        /// Checks if:
        /// - scheme is "mfa"
        /// - method is provided
        /// - label is provided
        /// </summary>
        public bool IsValid
        {
            get
            {
                return REGEX_MFA_URL.IsMatch(this.AbsoluteUri)
                    && this.Scheme == SCHEME_NAME;
            }
        }

        #endregion
    }
}
