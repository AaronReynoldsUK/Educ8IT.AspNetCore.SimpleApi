using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaUriBuilder
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public MfaUriBuilder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaUri"></param>
        public MfaUriBuilder(MfaUri mfaUri)
        {
            Set(mfaUri);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string SchemeName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; private set; }

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
        public MfaUriBuilder Reset()
        {
            SchemeName = String.Empty;
            Method = String.Empty;
            Label = String.Empty;
            Account = String.Empty;
            QueryParams = new NameValueCollection();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mfaUri"></param>
        /// <returns></returns>
        public MfaUriBuilder Set(MfaUri mfaUri)
        {
            SchemeName = mfaUri.SchemeName;
            Method = mfaUri.Method;
            Label = mfaUri.Label;
            Account = mfaUri.Account;

            if (QueryParams == null)
                QueryParams = new NameValueCollection();

            if (mfaUri.QueryParams == null)
                return this;

            foreach(var __key in mfaUri.QueryParams.AllKeys)
            {
                QueryParams.Add(__key, mfaUri.QueryParams[__key]);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        public MfaUriBuilder SetSchemeName(string schemeName)
        {
            SchemeName = schemeName;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public MfaUriBuilder SetMethod(string method)
        {
            Method = method;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public MfaUriBuilder SetLabel(string label)
        {
            Label = label;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MfaUriBuilder AddQuerySet(string key, string value)
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
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{SchemeName}://");
            sb.Append($"{WebUtility.UrlEncode(Method)}/");
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

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MfaUri Build()
        {
            return new MfaUri(this.ToString());
        }

        #endregion
    }
}
