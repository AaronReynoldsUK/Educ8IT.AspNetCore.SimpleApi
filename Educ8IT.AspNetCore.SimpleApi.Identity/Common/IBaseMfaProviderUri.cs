namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// Interface for BaseMfaProviderUri
    /// </summary>
    public interface IBaseMfaProviderUri
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        MfaDataUri DataUri { get; }

        /// <summary>
        /// Required scheme name for this Uri
        /// </summary>
        string SCHEME_NAME { get; }

        /// <summary>
        /// 
        /// </summary>
        string PATTERN_MFA_URL { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Text.RegularExpressions.Regex REGEX_MFA_URL { get; }

        /// <summary>
        /// Checks if scheme is correct and other validations as per scheme type
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Collections.Specialized.NameValueCollection QueryParams { get; }

        #endregion

        #region Member Functions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.Specialized.NameValueCollection GetQueryParameters(string queryString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetQueryValue(string keyName, string defaultValue = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customUri"></param>
        MfaDataUri ParseIntoMfaDataUri(System.Uri customUri);

        #endregion
    }
}
