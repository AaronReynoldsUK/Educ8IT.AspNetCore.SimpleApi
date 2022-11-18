using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.Identity;
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
    public class EmailMfaHandler : MfaHandler<MfaSchemeOptions>, IMfaHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        public EmailMfaHandler(IOptionsMonitor<MfaSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        { }

        #region IMfaHandler

        /// <inheritdoc/>
        public override Task<ActionResult> Prompt(HttpContext context, ApiUser user, ApiMfa apiMfa, string scheme)
        {
            //this.Options.
            // Generate a OTC

            // we need to know the recipient email + name
            // plus the OTC and the links required (if any)

            // Use the IEmailService to send a prompt

            // basically send the content that can be put inside a template
            // so we need to generate some basic wrapping HTML for the prompt
            // maybe some localisation??


            //if (MailToMfaUri.TryCreateFromCustomUriString("", out )
            //MailToMfaUri custom = 


            return base.Prompt(context, user, apiMfa, scheme);
        }

        /// <inheritdoc/>
        public override Task<ActionResult> Verify(HttpContext context, ApiUser user, ApiMfa apiMfa, string scheme, string code)
        {
            return base.Verify(context, user, apiMfa, scheme, code);
        }

        #endregion
    }
}
