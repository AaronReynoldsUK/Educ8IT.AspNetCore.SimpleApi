namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa.UriSchemes
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomMfaUri
    {
        #region Constants and Static Properties

        /// <summary>
        /// 
        /// </summary>
        public static string PATTERN_CUSTOM_URL { get; }

        /// <summary>
        /// 
        /// </summary>
        public static System.Text.RegularExpressions.Regex REGEX_CUSTOM_URL { get; }

        #endregion

        #region Instance Properties

        /// <summary>
        /// e.g. mailto, totp, sms, tel
        /// </summary>
        public abstract string MFA_SCHEME { get; }

        /// <summary>
        /// If supplied, limits the Query to only these keys
        /// </summary>
        public abstract string[] ALLOWED_QUERY_KEYS { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSchemeMatch { get; }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.Specialized.NameValueCollection QueryParameters { get; }

        #endregion

        #region Static Parsing Functions

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="customUri"></param>
        /// <returns></returns>
        public static bool TryCreateFromUri<T>(System.Uri uri, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uriString"></param>
        /// <param name="customUri"></param>
        /// <returns></returns>
        public static bool TryCreateFromUriString<T>(string uriString, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customUriString"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool TryCreateUriFromCustomUriString(string customUriString, out System.Uri uri)
        {
            uri = default(System.Uri);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customUriString"></param>
        /// <param name="customUri"></param>
        /// <returns></returns>
        public static bool TryCreateFromCustomUriString<T>(string customUriString, out T customUri) where T : struct, ICustomMfaUri
        {
            customUri = default(T);
            return false;
        }

        #endregion

        #region Static Functions



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static bool IsValid<T>(string uriString) where T : struct, ICustomMfaUri
        {
            return false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="customUriString"></param>
        ///// <param name="customUri"></param>
        ///// <returns></returns>
        //public static bool TryParse<T>(string customUriString, out T customUri) where T : struct, ICustomMfaUri
        //{
        //    customUri = default;

        //    return false;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uriString"></param>
        ///// <param name="uri"></param>
        ///// <returns></returns>
        //public static bool TryParse(string uriString, out System.Uri uri)
        //{
        //    uri = default;

        //    return false;
        //}

        #endregion

        #region Other Member Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetQueryValue(string keyName, string defaultValue = null);

        /// <summary>
        /// Converts Uri to specified Uri string format
        /// </summary>
        /// <returns></returns>
        public abstract string GetAsNativeUriString();

        /// <summary>
        /// Uri containing all information suitable for storing in a DataStore
        /// </summary>
        /// <returns></returns>
        public abstract string GetDataStoreUriString();

        /// <summary>
        /// Initialiser Uri containing secrets for setting up e.g. TOTP app
        /// </summary>
        /// <returns></returns>
        public abstract string GetInitUriString();

        /// <summary>
        /// Safe to share information used for displaying a prompt to the User
        /// </summary>
        /// <returns></returns>
        public abstract string GetPromptUriString();

        /// <summary>
        /// Contains a OTC for out-of-band transmission e.g. via email to confirm access to that Inbox
        /// </summary>
        /// <returns></returns>
        public abstract string GetOutOfBandUriString();

        #endregion
    }
}
