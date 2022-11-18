using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Patterns
    {
        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_IP_ADDRESS_V4_BYTE =
            "(?<byte>(?:2(?:[0-4][0-9]|5[0-5]))|(?:[0-1]?[0-9]?[0-9]))";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_IP_ADDRESS_V4 =
            $"({PATTERN_IP_ADDRESS_V4_BYTE}\\." +
            $"{PATTERN_IP_ADDRESS_V4_BYTE}\\." +
            $"{PATTERN_IP_ADDRESS_V4_BYTE}\\." +
            $"{PATTERN_IP_ADDRESS_V4_BYTE})";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_IP_ADDRESS_V4_COMPLETE =
            $"^{PATTERN_IP_ADDRESS_V4}$";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_URL_PROTOCOL = "(?<Protocol>\\w+)";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_SCHEME = $"{PATTERN_URL_PROTOCOL}:\\/\\/";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_DOMAIN_LABEL =
            "(?<domain_label>(?!\\-)(?!.*\\-)(?!\\d)(?!.*_.*)[\\w\\-]{1,63})";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_DOMAIN_NAME =
            $"(?<fqdn>(?:{PATTERN_DOMAIN_LABEL}\\.)*{PATTERN_DOMAIN_LABEL})";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_URL_ENCODED_CHARS = "A-Za-z0-9\\%\\.\\-_";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_AUTH =
            $"(?<auth>(?<user_name>[{PATTERN_URL_ENCODED_CHARS}]{1,63}):(?<user_password>[{PATTERN_URL_ENCODED_CHARS}]{1,63})@)";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_PATH_SEGMENT = "(?<path_segment>[{PATTERN_URL_ENCODED_CHARS}]{1,63})";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_PATH =
            $"(?<path>\\/(?:(?:{PATTERN_URL_PATH_SEGMENT}\\/)*{PATTERN_URL_PATH_SEGMENT}))";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_QUERY_PARAM_SET =
            $"(?:(?<key>[{PATTERN_URL_ENCODED_CHARS}]{1,255})" +
            $"(?:\\=(?<value>[{PATTERN_URL_ENCODED_CHARS}]{0,255})))";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_QUERY =
            $"(?:\\?(?<query>(?:{PATTERN_URL_QUERY_PARAM_SET}\\&)*{PATTERN_URL_QUERY_PARAM_SET}))";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL_FRAGMENT =
            $"(?:#(?<fragment>[{PATTERN_URL_ENCODED_CHARS}]{1,63}))";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_URL =
            $"^{PATTERN_URL_SCHEME}" +
            $"{PATTERN_URL_AUTH}?" +
            $"(?:{PATTERN_DOMAIN_NAME}|{PATTERN_IP_ADDRESS_V4})" +
            $"{PATTERN_URL_PATH}?" +
            $"{PATTERN_URL_QUERY}?" +
            $"{PATTERN_URL_FRAGMENT}?";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_EMAIL_ADDRESS_USER =
            "(?<user>(?![.+\\-])(?!.*\\.$)(?!.*\\.\\.)[\\w\\-.+]+)";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_EMAIL_ADDRESS_HOST =
            $"(?<host>{PATTERN_DOMAIN_NAME}|{PATTERN_IP_ADDRESS_V4})";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_EMAIL_ADDRESS =
            $"(?<email>{PATTERN_EMAIL_ADDRESS_USER}@{PATTERN_EMAIL_ADDRESS_HOST})";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_EMAIL_ADDRESS_COMPLETE =
            $"^{PATTERN_EMAIL_ADDRESS_USER}@{PATTERN_EMAIL_ADDRESS_HOST}$";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_DISPLAY_NAME_CHARS = @"a-zA-Z\.\-\s";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_DISPLAY_NAME_COMPLETE = $"^[{PATTERN_DISPLAY_NAME_CHARS}]{3,100}$";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_USERNAME_CHARS = @"\w\.\@";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_USERNAME_COMPLETE = $"^[{PATTERN_USERNAME_CHARS}]{3,100}$";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_PASSWORD_CHARS = @"\w\|\,\.\<\>\/\\\?\;\:\'\@\#\~\[\]\{\}\`\!\£\$\%\^\&\*\(\)\-\=\+";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_PASSWORD_COMPLETE = $"^[{PATTERN_PASSWORD_CHARS}]{8,100}$";

        /// <summary>
        /// 
        /// </summary>
        public const string PATTERN_TOKEN_CHARS = @"A-Za-z0-9\+\/\=";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_TOKEN_COMPLETE = $"^[{PATTERN_TOKEN_CHARS}]{16,1000}$";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_TOTP = $"^[0-9]{6,10}$";

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_UUID = $"^[a-fA-F0-9\\-]{32,36}$";
    }
}
