using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    public class MailToUri: CustomMfaUri
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class MailToUri_
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string URI_SCHEME_MAILTO = "mailto";

        /// <summary>
        /// 
        /// </summary>
        public const string MAILTO_PARAM_SUBJECT = "subject";

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
        public const string MAILTO_PARAM_BODY = "body";

        #endregion

        #region Patterns

        private static readonly string PATTERN_SCHEME_NAME = $"(?<scheme_name>{URI_SCHEME_MAILTO})";
        private static readonly string PATTERN_SCHEME = $"(?<scheme>{PATTERN_SCHEME_NAME}:)";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string PATTERN_MAILTO =
            $"^{PATTERN_SCHEME}({Patterns.PATTERN_EMAIL_ADDRESS})?({Patterns.PATTERN_URL_QUERY})?$";

        #endregion

        #region Regular Expressions

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_MAILTO = new Regex(
                PATTERN_MAILTO,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        #endregion

        #region Fields

        private List<string> _Keys;
        private List<string> _Values;

        #endregion

        #region Properties

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
        public string EmailAddress { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection QueryParams { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        public MailToUri(string uriString)
        {
            OriginalUri = uriString;

            Parse();
        }

        #endregion

        #region Private Member Functions

        private void Reset()
        {
            SchemeName = String.Empty;
            EmailAddress = String.Empty;
            QueryParams = new NameValueCollection();
            _Keys = new List<string>();
            _Values = new List<string>();
        }

        private void Parse()
        {
            Reset();

            if (OriginalUri == null)
                return;

            if (REGEX_MAILTO.IsMatch(OriginalUri))
            {
                var __mc = REGEX_MAILTO.Matches(OriginalUri);

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

                            case "email":
                                EmailAddress = WebUtility.UrlDecode(g.Value);
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
                    for (int x = 0; x < this._Keys.Count; x++)
                    {
                        this.QueryParams.Add(this._Keys[x], this._Values[x]);
                    }
                }
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
            return OriginalUri ?? base.ToString();
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
