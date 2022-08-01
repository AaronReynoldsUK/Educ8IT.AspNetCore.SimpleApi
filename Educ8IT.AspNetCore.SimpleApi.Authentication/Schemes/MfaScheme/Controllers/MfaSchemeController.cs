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
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.MfaScheme
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController("MfaSchemeController", "auth/mfa", "v{version:apiVersion}/auth/mfa")]
    public class MfaSchemeController
    {
        #region Fields

        private readonly ILogger _iLogger;
        private readonly HttpContext _httpContext;
        private readonly IApiMapperService _apiMapperService;
        private readonly ApiMapperOptions _apiMapperOptions;
        private readonly MfaOptions _mfaOptions;
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
        /// <param name="mfaOptions"></param>
        /// <param name="identityManagerService"></param>
        /// <param name="emailService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MfaSchemeController(
            ILogger<MfaSchemeController> iLogger,
            HttpContext httpContext,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions,
            IOptions<MfaOptions> mfaOptions,
            IIdentityManagerService identityManagerService,
            IEmailService emailService)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
            _mfaOptions = mfaOptions?.Value ?? throw new ArgumentNullException(nameof(mfaOptions));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("options")]        
        [AllowedResponseContentType("application/json")]
        [ResponseType(HttpStatusCode.OK)]
        [ResponseType(HttpStatusCode.BadRequest, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.Forbidden, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.InternalServerError, typeof(ProblemDetails))]
        [ResponseHeader("Cache-Control", "no-store")]
        [ResponseHeader("Pragma", "no-cache")]
        [Authenticate(EAuthenticationType.Identity)]
        public async Task<ActionResult> GetOptionsAsync()
        {
            try
            {
                if (_identityManagerService == null)
                    throw new CustomHttpException("Unable to access Identity Manager", HttpStatusCode.InternalServerError);

                if (!_identityManagerService.HasDbContext)
                    throw new CustomHttpException("Unable to access Identity Data Context", HttpStatusCode.InternalServerError);

                // Get User
                var __emailClaim = _httpContext.User.GetClaim(ClaimTypes.Email);
                if (__emailClaim == null)
                {
                    throw new Exception("Unable to locate user email address");
                }

                // Get User by Email Address or fail
                var __user = await _identityManagerService.GetUserByEmailAddressAsync(__emailClaim.Value);
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

                // Get allowed MFA methods
                var __userMfaOptions = await _identityManagerService.GetMfaEntriesByUserIdAsync(__user.Id);
                if (__userMfaOptions == null || __userMfaOptions.Count == 0)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "No MFA options available for user",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                var __results = __userMfaOptions.Select(s => new MfaOptionDto(s)).ToList();

                return ActionResult.GetPagedActionResult(HttpStatusCode.OK, __results, __results.Count);
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

                if (bearerTokenRequest.GrantType != MfaDefaults.RequestGrantName)
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

                // Get allowed MFA methods
                var __userMfaOptions = await _identityManagerService.GetMfaEntriesByUserIdAsync(__user.Id);
                if (__userMfaOptions == null || __userMfaOptions.Count == 0)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "No MFA options available for user",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!Guid.TryParse(bearerTokenRequest.MfaMethodId, out Guid mfaMethodId))
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "No sych MFA option available for user",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                var __selectedMethod = __userMfaOptions.FirstOrDefault(o => o.Id == mfaMethodId);
                if (__selectedMethod == null)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "No sych MFA option available for user",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Do MFA...
                // For Email / SMS, create a token and send back

                // For App, store interim key so verify can only accept the code once

                //byte[] __secretKey = Identity.OtpNet.KeyGeneration.GenerateRandomKey(Identity.OtpNet.OtpHashMode.Sha1);
                //string __secretKeyBase32 = Identity.OtpNet.Base32Encoding.ToString(__secretKey);


                // For TOTP...
                // secret key created when Auth App added to Mfa options (use User Identity manager)
                // params for all are a Uri, not a bar-delimited string

                // e.g. otpauth://totp/SimpleApi:email@email.com?secret={secret}&issuer={provider}&algorithm=SHA1&digits=6&period=30

                // or mailto://email@email.com for email MFA

                // or sms://+7538822102 for SMS MFA

                // or mailto://447538822102@sms-gateway.com&subject=PRESENT_THIS_NAME for SMS MFA

                // for push messages, would need Firebase or similar and/or native apps


                string __secretKeyBase32 = "GAWAX2V2CIYBX37PY5KOPDKS3V4MXBTG";
                byte[] __secretKey = Identity.OtpNet.Base32Encoding.ToBytes(__secretKeyBase32);

                var __totp = new Identity.OtpNet.Totp(__secretKey);
                var __ctotp = __totp.ComputeTotp();
                var __vtotp = __totp.VerifyTotp(__ctotp, out long timeStepMatched);

                System.Diagnostics.Debug.WriteLine(__vtotp);

                // Params = google_authenticator|{step}|{mode}|{size}|{secret}
                // Uri = otpauth://totp/SimpleApi:email@email.com?secret={secret}&issuer={provider}&algorithm=SHA1&digits=6&period=30

                var __tmp = "otpauth://totp/SimpleApi:email%40email.com?secret=secret&issuer=provider&algorithm=SHA1&digits=6&period=30";
                if (OtpUri.TryParse(__tmp, out OtpUri otpUri))
                {
                    System.Diagnostics.Debug.WriteLine(otpUri);
                }

                var __otpUri = new OtpUriBuilder()
                    .SetType(OtpUri.OTP_TYPE_TOTP)
                    .SetLabel("SimpleApiTest")
                    .SetAccount("aaron@email.com")
                    .AddQuerySet(OtpUri.OTP_PARAM_SECRET, "SeCrEt")
                    .AddQuerySet(OtpUri.OTP_PARAM_ISSUER, "Educ8IT")
                    .AddQuerySet(OtpUri.OTP_PARAM_PERIOD, "30")
                    .Build();

                System.Diagnostics.Debug.WriteLine(__otpUri.OriginalUri);
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

            await Task.CompletedTask;
            throw new NotImplementedException();
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
            await Task.CompletedTask;
            throw new NotImplementedException();

            // during verification, check if the OTC matches in the DB, if so, do not allow (single use restriction)

            // once verified, store the OTC as the token (to dissallow further use)
        }
    }
}
