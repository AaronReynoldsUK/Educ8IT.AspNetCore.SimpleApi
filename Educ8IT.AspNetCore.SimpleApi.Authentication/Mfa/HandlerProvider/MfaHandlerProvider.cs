using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaHandlerProvider : IMfaHandlerProvider
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemes"></param>
        public MfaHandlerProvider(IMfaSchemeProvider schemes)
        {
            Schemes = schemes;
        }

        #endregion

        #region Fields

        // handler instance cache, need to initialize once per request
        private Dictionary<string, IMfaHandler> _handlerMap = new Dictionary<string, IMfaHandler>(StringComparer.Ordinal);

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="IMfaHandlerProvider"/>.
        /// </summary>
        public IMfaSchemeProvider Schemes { get; }

        #endregion

        /// <inheritdoc/>
        public async Task<List<SimpleApi.Identity.EMfaMethod>> SupportedMethods()
        {
            var __schemes = await Schemes.GetAllSchemesAsync();

            return __schemes.Select(s => s.Method).Distinct().ToList();
        }

        /// <inheritdoc/>
        public async Task<IMfaHandler> GetHandlerAsync(HttpContext context, string mfaScheme)
        {
            if (_handlerMap.ContainsKey(mfaScheme))
            {
                return _handlerMap[mfaScheme];
            }

            var scheme = await Schemes.GetSchemeAsync(mfaScheme);
            if (scheme == null)
            {
                return null;
            }

            var handler = (context.RequestServices.GetService(scheme.HandlerType) ??
                ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType))
                as IMfaHandler;
            
            if (handler != null)
            {
                await handler.InitializeAsync(scheme, context);
                _handlerMap[mfaScheme] = handler;
            }
            return handler;
        }

        /// <inheritdoc/>
        public async Task<IMfaHandler> GetHandlerAsync(HttpContext context, SimpleApi.Identity.EMfaMethod method)
        {
            var scheme = await Schemes.GetSchemeForMethodAsync(method);

            if (scheme == null)
            {
                return null;
            }

            string mfaScheme = scheme.Name;

            if (_handlerMap.ContainsKey(mfaScheme))
            {
                return _handlerMap[mfaScheme];
            }

            var handler = (context.RequestServices.GetService(scheme.HandlerType) ??
                ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType))
                as IMfaHandler;

            if (handler != null)
            {
                await handler.InitializeAsync(scheme, context);
                _handlerMap[mfaScheme] = handler;
            }
            return handler;
        }
    }
}
