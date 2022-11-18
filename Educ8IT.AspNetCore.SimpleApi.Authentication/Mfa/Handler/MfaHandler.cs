using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Mfa
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MfaHandler<TOptions> : IMfaHandler where TOptions : MfaSchemeOptions, new()
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        public MfaHandler(IOptionsMonitor<TOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder)
        {
            OptionsMonitor = options;
            Logger = logger.CreateLogger(this.GetType().FullName);
            UrlEncoder = encoder;
        }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MfaScheme Scheme { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TOptions Options { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpContext Context { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected HttpRequest Request
        {
            get => Context.Request;
        }

        /// <summary>
        /// 
        /// </summary>
        protected HttpResponse Response
        {
            get => Context.Response;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        protected UrlEncoder UrlEncoder { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IOptionsMonitor<TOptions> OptionsMonitor { get; }

        #endregion

        #region IMfaHandler

        /// <inheritdoc/>
        public virtual async Task InitializeAsync(MfaScheme scheme, HttpContext context)
        {
            if (scheme == null)
            {
                throw new ArgumentNullException(nameof(scheme));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Scheme = scheme;
            Context = context;

            Options = OptionsMonitor.Get(Scheme.Name);

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual Task<string> GetCurrentOTC()
            => Task.FromResult(String.Empty);

        /// <inheritdoc/>
        public virtual Task<string> GetPromptUri(HttpContext context, string scheme)
            => Task.FromResult(String.Empty);

        /// <inheritdoc/>
        public virtual Task<string> GetProvisionUri(HttpContext context, string scheme)
            => Task.FromResult(String.Empty);

        /// <inheritdoc/>
        public virtual Task<bool> IsValidOTC(HttpContext context, string scheme, string code)
            => Task.FromResult(false);

        /// <inheritdoc/>
        public virtual Task<ActionResult> Prompt(HttpContext context, SimpleApi.Identity.ApiUser user, SimpleApi.Identity.ApiMfa apiMfa, string scheme)
            => Task.FromResult(ActionResult.NoContent());

        /// <inheritdoc/>
        public virtual Task<ActionResult> Verify(HttpContext context, SimpleApi.Identity.ApiUser user, SimpleApi.Identity.ApiMfa apiMfa, string scheme, string code)
            => Task.FromResult(ActionResult.NoContent());

        #endregion
    }
}
