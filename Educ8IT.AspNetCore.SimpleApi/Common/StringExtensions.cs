using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static string ReplaceIn(this string template, Dictionary<string, string> parameters)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var item in parameters)
            {
                if (String.IsNullOrEmpty(item.Key))
                    continue;

                template = template.Replace(item.Key, item.Value);
            }

            return template;
        }
    }
}
