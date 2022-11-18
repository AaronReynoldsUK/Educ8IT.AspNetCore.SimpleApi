using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMfaHandlerProvider
    {
        /// <summary>
        /// Returns a list of the supported <see cref="SimpleApi.Identity.EMfaMethod"/> methods 
        ///  handled by registered <see cref="IMfaHandler"/> handlers.
        /// </summary>
        /// <returns></returns>
        Task<List<SimpleApi.Identity.EMfaMethod>> SupportedMethods();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mfaScheme"></param>
        /// <returns></returns>
        Task<IMfaHandler> GetHandlerAsync(HttpContext context, string mfaScheme);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        Task<IMfaHandler> GetHandlerAsync(HttpContext context, SimpleApi.Identity.EMfaMethod method);
    }
}
