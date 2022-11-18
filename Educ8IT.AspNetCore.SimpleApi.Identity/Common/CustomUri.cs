using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CustomUri : ICustomUri
    {
        public string[] URL_SCHEMES_HANDLED => throw new NotImplementedException();

        public string PATTERN_CUSTOM_URL => throw new NotImplementedException();

        public Regex REGEX_CUSTOM_URL => throw new NotImplementedException();

        public string OriginalUri => throw new NotImplementedException();

        public string SchemeName => throw new NotImplementedException();

        public NameValueCollection QueryParams => throw new NotImplementedException();

        public void Parse(string uriString)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
