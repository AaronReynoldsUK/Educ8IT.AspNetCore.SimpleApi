using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public class MfaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MfaOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="schemes"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MfaMiddleware(RequestDelegate next, IMfaSchemeProvider schemes, IOptionsMonitor<MfaOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            Schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));

            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        public IMfaSchemeProvider Schemes { get; set; }

        public async Task Invoke(HttpContext context)
        {
            // Get endpoint, or skip to next Middleware
            var __endpoint = context.GetEndpoint() as RouteEndpoint;
            if (__endpoint == null)
            {
                await _next(context);
                return;
            }

            // Obtain Controlller + Action, or skip to next Middleware
            var __methodItem = __endpoint.Metadata.GetMetadata<AuthenticationAwareApiMethodItem>();
            if (__methodItem == null || __methodItem.ApiControllerItem == null)
            {
                await _next(context);
                return;
            }

            // Check if Controller + Action indicate MFA requirement

            if (!(__methodItem.ApiControllerItem is AuthenticationAwareApiControllerItem __controllerItem))
            {
                await _next(context);
                return;
            };

            if (__controllerItem.AuthenticateAttribute == null)
            {
                await _next(context);
                return;
            }

            if (__controllerItem.AuthenticateAttribute.AuthenticationTypeRequired.HasFlag(EAuthenticationType.MFA))
            {

            }

            // Obtain MFA status from user+tokens

            // Update user/principle with MFA status





            // Optional Features...

            // Allow handlers to get involved...
            var handlers = context.RequestServices.GetRequiredService<IMfaHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IMfaHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    return;
                }

                 
            }

            await _next(context);
        }
    }
}
