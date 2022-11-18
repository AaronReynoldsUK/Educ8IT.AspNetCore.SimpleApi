using Educ8IT.AspNetCore.SimpleApi.Identity.Common;
using System;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa.UriSchemes
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaProviderUri_OtpApp : BaseMfaProviderUri
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public MfaProviderUri_OtpApp(MfaDataUri uri) : base(uri) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customUriString"></param>
        public MfaProviderUri_OtpApp(string customUriString) : base(customUriString)
        {
            // Validate ??
        }

        #endregion

        #region Constants

        private const string _SCHEME_NAME = "otpauth";

        #endregion

        #region Static Fields

        private static string[] _OTP_HANDLED =
        {
            "hotp",
            "totp"
        };

        private static readonly string PATTERN_MFA_SCHEME_NAME = $"(?<scheme_name>{_SCHEME_NAME})";
        private static readonly string PATTERN_MFA_SCHEME = $"(?<scheme>{PATTERN_MFA_SCHEME_NAME}://)";
        private static readonly string PATTERN_MFA_OTP_TYPE = $"(?<type>{String.Join("|", _OTP_HANDLED)}://)";
        private static readonly string PATTERN_MFA_LABEL = $"(?<label>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";
        private static readonly string PATTERN_ACCOUNT = $"(?<account>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{{1,255}})";

        private static readonly string _PATTERN_MFA_URL =
            $"^{PATTERN_MFA_SCHEME}/{PATTERN_MFA_OTP_TYPE}/{PATTERN_MFA_LABEL}(?:\\:{PATTERN_ACCOUNT})?{Patterns.PATTERN_URL_QUERY}?$";

        private static readonly System.Text.RegularExpressions.Regex _REGEX_MFA_URL =
            new Regex(
                _PATTERN_MFA_URL,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string SCHEME_NAME
        {
            get { return _SCHEME_NAME; }
        }

        /// <inheritdoc/>
        public override string PATTERN_MFA_URL
        {
            get
            {
                return _PATTERN_MFA_URL;
            }
        }

        /// <inheritdoc/>
        public override Regex REGEX_MFA_URL
        {
            get
            {
                return _REGEX_MFA_URL;
            }
        }

        /// <inheritdoc/>
        public override bool IsValid
        {
            get
            {
                return DataUri.IsValid;
            }
        }

        #endregion

        #region Member Functions

        /// <inheritdoc/>
        public override MfaDataUri ParseIntoMfaDataUri(Uri customUri)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
