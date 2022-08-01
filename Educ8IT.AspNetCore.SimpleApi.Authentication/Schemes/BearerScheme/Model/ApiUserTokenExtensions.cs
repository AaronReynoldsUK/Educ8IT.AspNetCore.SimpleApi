using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiUserTokenExtensions
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="apiUserToken"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static Dictionary<string, object> GetExtendedData(this Identity.ApiUserToken apiUserToken)
        //{
        //    if (apiUserToken == null)
        //        throw new ArgumentNullException(nameof(apiUserToken));

        //    apiUserToken.ExtendedData = !String.IsNullOrEmpty(apiUserToken.ExtendedDataInDb)
        //        ? JsonConvert.DeserializeObject<Dictionary<string, object>>(apiUserToken.ExtendedDataInDb)
        //        : default;

        //    return apiUserToken.ExtendedData;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="apiUserToken"></param>
        ///// <param name="extendedData"></param>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static void SetExtendedData(this Identity.ApiUserToken apiUserToken, Dictionary<string, object> extendedData)
        //{
        //    if (apiUserToken == null)
        //        throw new ArgumentNullException(nameof(apiUserToken));

        //    apiUserToken.ExtendedDataInDb = extendedData != null
        //        ? JsonConvert.SerializeObject(extendedData)
        //        : null;
        //}
    }
}

