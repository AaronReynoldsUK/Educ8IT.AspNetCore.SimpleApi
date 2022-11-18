using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegularExpressions
    {
        /// <summary>
        /// IP Address (v4) regular expression
        /// </summary>
        public static Regex REGEX_IP_ADDRESS_V4 = new Regex(
            Patterns.PATTERN_IP_ADDRESS_V4_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// URL regular expression
        /// </summary>
        public static Regex REGEX_URL = new Regex(
            Patterns.PATTERN_URL,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_EMAIL_ADDRESS = new Regex(
            Patterns.PATTERN_EMAIL_ADDRESS_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_DISPLAY_NAME = new Regex(
            Patterns.PATTERN_DISPLAY_NAME_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_USERNAME = new Regex(
            Patterns.PATTERN_USERNAME_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_PASSWORD = new Regex(
            Patterns.PATTERN_PASSWORD_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_TOKEN = new Regex(
            Patterns.PATTERN_TOKEN_COMPLETE,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_TOTP = new Regex(
            Patterns.PATTERN_TOTP,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        
        /// <summary>
        /// 
        /// </summary>
        public static Regex REGEX_UUID = new Regex(
            Patterns.PATTERN_UUID,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
    }
}
