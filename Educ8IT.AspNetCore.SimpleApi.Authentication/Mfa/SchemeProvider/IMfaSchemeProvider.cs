using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// Responsible for managing what MfaSchemes are supported.
    /// </summary>
    public interface IMfaSchemeProvider
    {
        /// <summary>
        /// Returns all currently registered <see cref="MfaScheme"/>s.
        /// </summary>
        /// <returns>All currently registered <see cref="MfaScheme"/>s.</returns>
        Task<IEnumerable<MfaScheme>> GetAllSchemesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        Task<MfaScheme> GetSchemeForMethodAsync(SimpleApi.Identity.EMfaMethod method);

        /// <summary>
        /// Returns the <see cref="MfaScheme"/> matching the name, or null.
        /// </summary>
        /// <param name="name">The name of the MfaScheme.</param>
        /// <returns>The scheme or null if not found.</returns>
        Task<MfaScheme> GetSchemeAsync(string name);

        /// <summary>
        /// Returns the scheme that will be used by default for MFA if no preference provided
        /// </summary>
        /// <returns>The scheme that will be used by default for MFA if no preference provided</returns>
        Task<MfaScheme> GetDefaultSchemeAsync();

        /// <summary>
        /// Registers a scheme for use by <see cref="IMfaService"/>. 
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        void AddScheme(MfaScheme scheme);

        /// <summary>
        /// Removes a scheme, preventing it from being used by <see cref="IMfaService"/>.
        /// </summary>
        /// <param name="name">The name of the MfaScheme being removed.</param>
        void RemoveScheme(string name);

        /// <summary>
        /// Returns the schemes in priority order for request handling.
        /// </summary>
        /// <returns>The schemes in priority order for request handling</returns>
        Task<IEnumerable<MfaScheme>> GetRequestHandlerSchemesAsync();
    }
}
