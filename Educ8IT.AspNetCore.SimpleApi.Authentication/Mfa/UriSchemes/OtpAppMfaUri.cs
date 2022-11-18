using Educ8IT.AspNetCore.SimpleApi.Identity.Common;
using System;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa.UriSchemes
{
    
    /// <summary>
    /// Use this scheme to send configuration to TOTP App
    /// </summary>
    public class OtpAppMfaUri : CustomMfaUri
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        public OtpAppMfaUri(string uriString) : base(uriString) { }

        #endregion

        #region Constants

        private static string[] _URL_SCHEMES_HANDLED =
        {
            "otpauth"
        };

        private static readonly string _PATTERN_SCHEME = $"(?<scheme>{String.Join("|", _URL_SCHEMES_HANDLED)})";
        private static readonly string _PATTERN_MFA_METHOD = $"(?<mfa_method>[{Patterns.PATTERN_URL_ENCODED_CHARS}]{1,255})";

        private static string _PATTERN_CUSTOM_URL = $"^{_PATTERN_SCHEME}://{_PATTERN_MFA_METHOD}/({Patterns.PATTERN_URL_QUERY})?$";

        #endregion

        #region Fields

        private string _mfaMethod = null;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string[] URL_SCHEMES_HANDLED
        {
            get
            {
                return _URL_SCHEMES_HANDLED;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new static string PATTERN_CUSTOM_URL
        {
            get
            {
                return _PATTERN_CUSTOM_URL;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MfaMethod
        {
            get { return _mfaMethod ??= GetMfaMethod(); }
        }

        #endregion

        #region Private Member Functions

        private string GetMfaMethod()
        {
            return this.Host;
        }

        #endregion

        #region Uri Exports

        /// <summary>
        /// Initialiser Uri containing secrets for setting up e.g. TOTP app
        /// </summary>
        /// <returns></returns>
        public override string GetInitUriString() { return null; }

        /// <summary>
        /// Safe to share information used for displaying a prompt to the User
        /// </summary>
        /// <returns></returns>
        public override string GetPromptUriString() { return null; }

        /// <summary>
        /// Contains a OTC for out-of-band transmission e.g. via email to confirm access to that Inbox
        /// </summary>
        /// <returns></returns>
        public override string GetOutOfBandUriString() { return null; }

        #endregion
    }
}
