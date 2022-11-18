using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class OtpUriBuilder
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection QueryParams { get; private set; }

        #endregion

        #region Public Member Functions

        /// <summary>
        /// 
        /// </summary>
        public OtpUriBuilder Reset()
        {
            SchemeName = String.Empty;
            Type = String.Empty;
            Label = String.Empty;
            Account = String.Empty;
            QueryParams = new NameValueCollection();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        public OtpUriBuilder SetSchemeName(string schemeName)
        {
            SchemeName = schemeName;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public OtpUriBuilder SetType(string type)
        {
            Type = type;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public OtpUriBuilder SetLabel(string label)
        {
            Label = label;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public OtpUriBuilder SetAccount(string account)
        {
            Account = account;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OtpUriBuilder AddQuerySet(string key, string value)
        {
            if (QueryParams == null)
                QueryParams = new NameValueCollection();

            QueryParams.Add(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OtpUri Build()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{SchemeName}://");
            sb.Append($"{WebUtility.UrlEncode(Type)}/");
            sb.Append($"{WebUtility.UrlEncode(Label)}");

            if (!String.IsNullOrEmpty(Account))
            {
                sb.Append($":{WebUtility.UrlEncode(Account)}");
            }

            if (QueryParams != null && QueryParams.Count > 0)
            {
                List<string> __paramSet = new List<string>();
                foreach (var key in QueryParams.AllKeys)
                {
                    __paramSet.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(QueryParams[key])}");
                }
                if (__paramSet != null && __paramSet.Count > 0)
                {
                    sb.Append("?");
                    sb.Append(String.Join("&", __paramSet.ToArray()));
                }
            }

            return new OtpUri(sb.ToString());
        }

        #endregion
    }
}
