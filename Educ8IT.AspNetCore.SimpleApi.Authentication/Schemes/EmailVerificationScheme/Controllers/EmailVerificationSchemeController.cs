using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Communication;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Identity;
using Educ8IT.AspNetCore.SimpleApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.EmailVerificationScheme
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController("EmailVerificationSchemeController", "auth/email", "v{version:apiVersion}/auth/email")]
    public class EmailVerificationSchemeController
    {
        #region Fields

        private readonly ILogger _iLogger;
        private readonly HttpContext _httpContext;
        private readonly IApiMapperService _apiMapperService;
        private readonly ApiMapperOptions _apiMapperOptions;
        private readonly EmailVerificationOptions _emailVerificationOptions;
        private readonly IIdentityManagerService _identityManagerService;
        private readonly IEmailService _emailService;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLogger"></param>
        /// <param name="httpContext"></param>
        /// <param name="apiMapperService"></param>
        /// <param name="apiMapperOptions"></param>
        /// <param name="emailVerificationOptions"></param>
        /// <param name="identityManagerService"></param>
        /// <param name="emailService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public EmailVerificationSchemeController(
            ILogger<EmailVerificationSchemeController> iLogger,
            HttpContext httpContext,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions,
            IOptions<EmailVerificationOptions> emailVerificationOptions,
            IIdentityManagerService identityManagerService,
            IEmailService emailService)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
            _emailVerificationOptions = emailVerificationOptions?.Value ?? throw new ArgumentNullException(nameof(emailVerificationOptions));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bearerTokenRequest"></param>
        /// <returns></returns>
        [HttpPost("request")]
        [AllowedRequestContentType("application/x-www-form-urlencoded")]
        [AllowedResponseContentType("application/json")]
        [ResponseType(HttpStatusCode.Accepted)]
        [ResponseType(HttpStatusCode.BadRequest, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.Forbidden, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.InternalServerError, typeof(ProblemDetails))]
        [ResponseHeader("Cache-Control", "no-store")]
        [ResponseHeader("Pragma", "no-cache")]
        [Authenticate(EAuthenticationType.Identity)]
        public async Task<ActionResult> RequestAsync([FromBody] BearerTokenRequest bearerTokenRequest)
        {
            try
            {
                if (_identityManagerService == null)
                    throw new CustomHttpException("Unable to access Identity Manager", HttpStatusCode.InternalServerError);

                if (!_identityManagerService.HasDbContext)
                    throw new CustomHttpException("Unable to access Identity Data Context", HttpStatusCode.InternalServerError);

                if (bearerTokenRequest == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Missing or invalid: {nameof(bearerTokenRequest)} in request body",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (bearerTokenRequest.GrantType != EmailVerificationDefaults.RequestGrantName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid grant type: {bearerTokenRequest.GrantType}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!bearerTokenRequest.IsValid)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        bearerTokenRequest.GetValidationProblemDetails());
                }

                // Get User
                var __emailClaim = _httpContext.User.GetClaim(ClaimTypes.Email);
                if (__emailClaim == null)
                {
                    throw new Exception("Unable to locate user email address");
                }

                if (__emailClaim.Value != bearerTokenRequest.UserName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid email address: {bearerTokenRequest.UserName}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Get User by UserName or fail
                var __user = await _identityManagerService.GetUserByUserNameAsync(bearerTokenRequest.UserName)
                    ?? await _identityManagerService.GetUserByEmailAddressAsync(bearerTokenRequest.UserName);
                if (__user == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "User authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Remove previous tokens
                await _identityManagerService.RemoveUserTokens(__user, false, EmailVerificationDefaults.RequestTokenName);

                // Generate Email Verification token...
                var __verificationToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    EmailVerificationDefaults.RequestTokenName,
                    EmailVerificationDefaults.RequestTokenTTL,
                    null);

                if (__verificationToken == null)
                {
                    return new ActionResult(HttpStatusCode.InternalServerError,
                        new ProblemDetails()
                        {
                            Detail = "Unable to generate confirmation token",
                            StatusCode = HttpStatusCode.InternalServerError,
                            Title = "Error"
                        });
                }


                // Use Email Service to send email...
                var __recipients = new List<System.Net.Mail.MailAddress>(new[]
                {
                        new System.Net.Mail.MailAddress(__user.EmailAddress, __user.DisplayName)
                    });
                var __replacements = new Dictionary<string, string>
                {
                    { "$$DISPLAY_NAME$$", __user.DisplayName },
                    { "$$EMAIL_ADDRESS$", __user.EmailAddress },
                    {  "$$CONFIRMATION_CODE$$", __verificationToken.Token }
                };

                string __emailBody = _emailService.ReplaceIn(_emailVerificationOptions.EmailBody, __replacements);

                if (await _emailService.SendEmailAsync(
                    _emailVerificationOptions.EmailSubject, __emailBody, _emailVerificationOptions.EmailBodyIsHtml, 
                    __recipients, null, null))
                {
                    return new ActionResult(HttpStatusCode.Accepted);
                }
                else
                {
                    return new ActionResult(HttpStatusCode.InternalServerError,
                        new ProblemDetails()
                        {
                            Detail = "Unable to send confirmation email",
                            StatusCode = HttpStatusCode.InternalServerError,
                            Title = "Error"
                        });
                }
            }
            catch (Exception ex)
            {
                this._iLogger.LogError(ex.Message);

                return new ActionResult(HttpStatusCode.InternalServerError, new ProblemDetails()
                {
                    Detail = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Title = "Error"
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bearerTokenRequest"></param>
        /// <returns></returns>
        [HttpPut("verify")]
        [AllowedRequestContentType("application/x-www-form-urlencoded")]
        [AllowedResponseContentType("application/json")]
        [ResponseType(HttpStatusCode.Accepted)]
        [ResponseType(HttpStatusCode.BadRequest, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.Forbidden, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.InternalServerError, typeof(ProblemDetails))]
        [ResponseHeader("Cache-Control", "no-store")]
        [ResponseHeader("Pragma", "no-cache")]
        [Authenticate(EAuthenticationType.Identity)]
        public async Task<ActionResult> VerifyAsync([FromBody] BearerTokenRequest bearerTokenRequest)
        {
            try
            {
                if (_identityManagerService == null)
                    throw new CustomHttpException("Unable to access Identity Manager", HttpStatusCode.InternalServerError);

                if (!_identityManagerService.HasDbContext)
                    throw new CustomHttpException("Unable to access Identity Data Context", HttpStatusCode.InternalServerError);

                if (bearerTokenRequest == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Missing or invalid: {nameof(bearerTokenRequest)} in request body",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (bearerTokenRequest.GrantType != EmailVerificationDefaults.VerifyGrantName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid grant type: {bearerTokenRequest.GrantType}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!bearerTokenRequest.IsValid)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        bearerTokenRequest.GetValidationProblemDetails());
                }

                // Get User
                var __emailClaim = _httpContext.User.GetClaim(ClaimTypes.Email);
                if (__emailClaim == null)
                {
                    throw new Exception("Unable to locate user email address");
                }

                if (__emailClaim.Value != bearerTokenRequest.UserName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid email address: {bearerTokenRequest.UserName}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Get User by UserName or fail
                var __user = await _identityManagerService.GetUserByUserNameAsync(bearerTokenRequest.UserName)
                    ?? await _identityManagerService.GetUserByEmailAddressAsync(bearerTokenRequest.UserName);
                if (__user == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "User authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Get Email Verification token
                var __verificationToken = await _identityManagerService.GetTokenByTokenValueAsync(
                    bearerTokenRequest.ConfirmationToken,
                    EmailVerificationDefaults.RequestTokenName);

                if (__verificationToken == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "Email authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (__verificationToken.IsExpired)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "Email authentication expired",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Remove previous tokens
                await _identityManagerService.RemoveUserTokens(__user, false, EmailVerificationDefaults.VerifyTokenName);

                // Remove request token too
                await _identityManagerService.RemoveUserTokens(__user, false, EmailVerificationDefaults.RequestTokenName);

                var __confirmationToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    EmailVerificationDefaults.VerifyTokenName,
                    null,
                    null);

                return new ActionResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                this._iLogger.LogError(ex.Message);

                return new ActionResult(HttpStatusCode.InternalServerError, new ProblemDetails()
                {
                    Detail = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Title = "Error"
                });
            }
        }
    }
}
