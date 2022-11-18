using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// Implements <see cref="IMfaService"/>.
    /// </summary>
    public class MfaService : IMfaService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="handlers"></param>
        /// <param name="options"></param>
        public MfaService(IMfaSchemeProvider schemes,
            IMfaHandlerProvider handlers,
            IOptions<MfaOptions> options)
        {
            Schemes = schemes;
            Handlers = handlers;
            Options = options.Value;
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Used to lookup MfaSchemes.
        /// </summary>
        public IMfaSchemeProvider Schemes { get; }

        /// <summary>
        /// Used to resolve IMfaHandler instances.
        /// </summary>
        public IMfaHandlerProvider Handlers { get; }

        /// <summary>
        /// The <see cref="MfaOptions"/>.
        /// </summary>
        public MfaOptions Options { get; }

        #endregion

        #region IMfaService

        /// <inheritdoc/>
        public virtual async Task<string> GetSchemeName(HttpContext context, SimpleApi.Identity.EMfaMethod method)
        {
            var scheme = await Schemes.GetSchemeForMethodAsync(method);
            if (scheme == null)
            {
                throw new InvalidOperationException($"No MfaScheme of type {method} is available");
            }

            return scheme.Name;
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetCurrentOTC(HttpContext context, string scheme)
        {
            if (scheme == null)
            {
                var defaultScheme = await Schemes.GetDefaultSchemeAsync();
                scheme = defaultScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No MfaScheme was specified"); //, and there was no DefaultMfaScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.GetCurrentOTC();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> IsValidOTC(HttpContext context, string scheme, string code)
        {
            if (scheme == null)
            {
                var defaultScheme = await Schemes.GetDefaultSchemeAsync();
                scheme = defaultScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No MfaScheme was specified"); //, and there was no DefaultMfaScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.IsValidOTC(context, scheme, code);
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetProvisionUri(HttpContext context, string scheme)
        {
            if (scheme == null)
            {
                var defaultScheme = await Schemes.GetDefaultSchemeAsync();
                scheme = defaultScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No MfaScheme was specified"); //, and there was no DefaultMfaScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.GetProvisionUri(context, scheme);
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetPromptUri(HttpContext context, string scheme)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (scheme == null)
            {
                var defaultScheme = await Schemes.GetDefaultSchemeAsync();
                scheme = defaultScheme?.Name;
                if (scheme == null)
                {
                    throw new InvalidOperationException($"No MfaScheme was specified"); //, and there was no DefaultMfaScheme found. The default schemes can be set using either AddAuthentication(string defaultScheme) or AddAuthentication(Action<AuthenticationOptions> configureOptions).");
                }
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.GetPromptUri(context, scheme);
        }

        /// <inheritdoc/>
        public virtual async Task<ActionResult> Prompt(HttpContext context, SimpleApi.Identity.ApiUser user, SimpleApi.Identity.ApiMfa apiMfa)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (apiMfa == null)
                throw new ArgumentNullException(nameof(apiMfa));

            // Get scheme from preference
            var scheme = await GetSchemeName(context, apiMfa.Method);
            if (scheme == null)
            {
                throw new InvalidOperationException($"No matching MfaScheme was available");
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.Prompt(context, user, apiMfa, scheme);
        }

        /// <inheritdoc/>
        public virtual async Task<ActionResult> Verify(HttpContext context, SimpleApi.Identity.ApiUser user, SimpleApi.Identity.ApiMfa apiMfa, string code)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (apiMfa == null)
                throw new ArgumentNullException(nameof(apiMfa));

            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            // Get scheme from preference
            var scheme = await GetSchemeName(context, apiMfa.Method);
            if (scheme == null)
            {
                throw new InvalidOperationException($"No matching MfaScheme was available");
            }

            var handler = await Handlers.GetHandlerAsync(context, scheme);
            if (handler == null)
            {
                throw await CreateMissingHandlerException(scheme);
            }

            return await handler.Verify(context, user, apiMfa, scheme, code);
        }

        #endregion

        private async Task<Exception> CreateMissingHandlerException(string scheme)
        {
            var schemes = string.Join(", ", (await Schemes.GetAllSchemesAsync()).Select(sch => sch.Name));

            var footer = $" Did you forget to call AddMfa().Add[SomeAuthHandler](\"{scheme}\",...)?";

            if (string.IsNullOrEmpty(schemes))
            {
                return new InvalidOperationException(
                    $"No MFA handlers are registered." + footer);
            }

            return new InvalidOperationException(
                $"No MFA handler is registered for the scheme '{scheme}'. The registered schemes are: {schemes}." + footer);
        }
    }
}
