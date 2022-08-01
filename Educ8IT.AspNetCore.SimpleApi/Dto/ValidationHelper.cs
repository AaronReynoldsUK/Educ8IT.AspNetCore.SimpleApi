using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Educ8IT.AspNetCore.SimpleApi.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="regularExpression"></param>
        /// <param name="allowEmpty"></param>
        /// <returns></returns>
        public static bool IsValid(
            this string dataIn,
            Regex regularExpression,
            bool allowEmpty)
        {
            if (regularExpression == null)
                throw new ArgumentNullException(nameof(regularExpression));

            if (String.IsNullOrEmpty(dataIn))
            {
                return allowEmpty;
            }

            return regularExpression.IsMatch(dataIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="allowedValues"></param>
        /// <param name="allowEmpty"></param>
        /// <param name="caseInsensitive"></param>
        /// <returns></returns>
        public static bool IsValid(
            this string dataIn,
            string[] allowedValues,
            bool allowEmpty,
            bool caseInsensitive)
        {
            if (allowedValues == null)
                throw new ArgumentNullException(nameof(allowedValues));

            if (String.IsNullOrEmpty(dataIn))
            {
                return allowEmpty;
            }

            return caseInsensitive
                ? allowedValues.Contains(dataIn, StringComparer.CurrentCultureIgnoreCase)
                : allowedValues.Contains(dataIn, StringComparer.CurrentCulture);
        }
    }
}
