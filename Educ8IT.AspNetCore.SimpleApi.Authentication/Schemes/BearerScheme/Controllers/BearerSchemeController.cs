using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
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

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.BearerScheme
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController("BearerSchemeController", "auth/bearer", "v{version:apiVersion}/auth/bearer")]
    public class BearerSchemeController
    {
        private readonly ILogger _iLogger;
        private readonly HttpContext _httpContext;
        private readonly IApiMapperService _apiMapperService;
        private readonly ApiMapperOptions _apiMapperOptions;
        private readonly BearerAuthenticationOptions _bearerAuthenticationOptions;
        private readonly IIdentityManagerService _identityManagerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLogger"></param>
        /// <param name="httpContext"></param>
        /// <param name="apiMapperService"></param>
        /// <param name="apiMapperOptions"></param>
        /// <param name="bearerAuthenticationOptions"></param>
        /// <param name="identityManagerService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BearerSchemeController(
            ILogger<BearerSchemeController> iLogger,
            HttpContext httpContext,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions,
            IOptions<BearerAuthenticationOptions> bearerAuthenticationOptions,
            IIdentityManagerService identityManagerService)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
            _bearerAuthenticationOptions = bearerAuthenticationOptions?.Value ?? throw new ArgumentNullException(nameof(bearerAuthenticationOptions));
            _identityManagerService = identityManagerService ?? throw new ArgumentNullException(nameof(identityManagerService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bearerTokenRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [AllowedRequestContentType("application/x-www-form-urlencoded")]
        [AllowedResponseContentType("application/json")]
        [ResponseType(HttpStatusCode.OK, typeof(BearerTokenResponse))]
        [ResponseType(HttpStatusCode.BadRequest, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.Forbidden, typeof(ProblemDetails))]
        [ResponseType(HttpStatusCode.InternalServerError, typeof(ProblemDetails))]
        [ResponseHeader("Cache-Control", "no-store")]
        [ResponseHeader("Pragma", "no-cache")]
        public async Task<ActionResult> LoginAsync([FromBody] BearerTokenRequest bearerTokenRequest)
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

                if (bearerTokenRequest.GrantType != BearerAuthenticationDefaults.RequestGrantName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid grant_type: {bearerTokenRequest.GrantType}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!bearerTokenRequest.IsValid)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        bearerTokenRequest.GetValidationProblemDetails());
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

                var __canSignIn = await _identityManagerService.CanSignInAsync(__user);
                if (!__canSignIn)
                {
                    return new ActionResult(HttpStatusCode.Forbidden,
                        new ProblemDetails()
                        {
                            Detail = "User authentication blocked",
                            StatusCode = HttpStatusCode.Forbidden,
                            Title = "Error"
                        });
                }

                var __isValidPwd = await _identityManagerService.IsValidPasswordAsync(__user, bearerTokenRequest.Password);
                if (!__isValidPwd)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "User authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                // Remove Transient user tokens
                await _identityManagerService.RemoveUserTokens(__user, true);

                // Add any further Request/Context info to the KVP set here
                Dictionary<string, object> __kvp = new Dictionary<string, object>
                {
                    {
                        BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp,
                        _httpContext.Connection.RemoteIpAddress.ToString()
                    }
                };

                // Tokens
                var __newRefreshToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    BearerAuthenticationDefaults.RefreshTokenName,
                    BearerAuthenticationDefaults.RefreshTokenTTL,
                    __kvp);

                var __newAccessToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    BearerAuthenticationDefaults.AccessTokenName,
                    BearerAuthenticationDefaults.AccessTokenTTL,
                    __kvp);

                // Prepare Principle
                var __claimsPrinciple = _identityManagerService.GetClaimsPrinciple(
                    __user,
                    BearerAuthenticationDefaults.AuthenticationScheme);
                
                var __authProperties = new AuthenticationProperties();
                __authProperties.SetParameter(BearerAuthenticationDefaults.AccessTokenName, __newAccessToken);

                // Sign In with Claims
                await _httpContext.SignInAsync(
                    BearerAuthenticationDefaults.AuthenticationScheme,
                    __claimsPrinciple,
                    __authProperties);

                // Send auth token to Client
                var __bearerToken = BearerTokenResponse
                    .GetAccessToken(__newAccessToken, __newRefreshToken);

                return new ActionResult(HttpStatusCode.OK, __bearerToken);
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
        [HttpPost("refresh")]
        [AllowAnonymous]
        [AllowedRequestContentType("application/x-www-form-urlencoded")]
        [AllowedResponseContentType("application/json")]
        public async Task<ActionResult> ReAuthenticate(
            [FromBody] BearerTokenRequest bearerTokenRequest)
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

                if (bearerTokenRequest.GrantType != BearerAuthenticationDefaults.RefreshGrantName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid grant_type: {bearerTokenRequest.GrantType}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!bearerTokenRequest.IsValid)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        bearerTokenRequest.GetValidationProblemDetails());
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

                var __canSignIn = await _identityManagerService.CanSignInAsync(__user);
                if (!__canSignIn)
                {
                    return new ActionResult(HttpStatusCode.Forbidden,
                        new ProblemDetails()
                        {
                            Detail = "User authentication blocked",
                            StatusCode = HttpStatusCode.Forbidden,
                            Title = "Error"
                        });
                }

                var __refreshToken = await _identityManagerService.GetTokenByTokenValueAsync(
                    bearerTokenRequest.RefreshToken,
                    BearerAuthenticationDefaults.RefreshTokenName);

                if (__refreshToken == null || __refreshToken.IsExpired)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "User authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (__refreshToken.UserId != __user.Id)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = "User authentication failed",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (__refreshToken.ExtendedData == null)
                {
                    return new ActionResult(HttpStatusCode.InternalServerError,
                        new ProblemDetails()
                        {
                            Detail = "User data was invalid. Please re-authenticate.",
                            StatusCode = HttpStatusCode.InternalServerError,
                            Title = "Error"
                        });
                }

                // Validate IP against props in refresh token
                if (__refreshToken.ExtendedData.ContainsKey(BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp))
                {
                    string __remoteIpAddress = __refreshToken.ExtendedData.GetValueOrDefault(BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp)?.ToString() ?? null;
                    if (__remoteIpAddress != _httpContext.Connection.RemoteIpAddress.ToString())
                    {
                        return new ActionResult(HttpStatusCode.BadRequest,
                            new ProblemDetails()
                            {
                                Detail = "User authentication failed",
                                StatusCode = HttpStatusCode.BadRequest,
                                Title = "Error"
                            });
                    }
                }

                // Remove Transient user tokens
                await _identityManagerService.RemoveUserTokens(__user, true);

                // Add any further Request/Context info to the KVP set here
                Dictionary<string, object> __kvp = new Dictionary<string, object>
                {
                    {
                        BearerAuthenticationDefaults.ExtendedDataKey_RemoteIp,
                        _httpContext.Connection.RemoteIpAddress.ToString()
                    }
                };

                // Tokens
                var __newRefreshToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    BearerAuthenticationDefaults.RefreshTokenName,
                    BearerAuthenticationDefaults.RefreshTokenTTL,
                    __kvp);

                var __newAccessToken = await _identityManagerService.NewTokenAsync(
                    __user,
                    BearerAuthenticationDefaults.AccessTokenName,
                    BearerAuthenticationDefaults.AccessTokenTTL,
                    __kvp);

                // Prepare Principle
                var __claimsPrinciple = _identityManagerService.GetClaimsPrinciple(
                    __user,
                    BearerAuthenticationDefaults.AuthenticationScheme);

                var __authProperties = new AuthenticationProperties();
                __authProperties.SetParameter(BearerAuthenticationDefaults.AccessTokenName, __newAccessToken);

                // Sign In with Claims
                await _httpContext.SignInAsync(
                    BearerAuthenticationDefaults.AuthenticationScheme,
                    __claimsPrinciple,
                    __authProperties);

                // Send auth token to Client
                var __bearerToken = BearerTokenResponse
                    .GetAccessToken(__newAccessToken, __newRefreshToken);

                return new ActionResult(HttpStatusCode.OK, __bearerToken);
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
        [HttpDelete("logout")]
        [AllowAnonymous]
        [AllowedRequestContentType("application/x-www-form-urlencoded")]
        [AllowedResponseContentType("application/json")]
        public async Task<ActionResult> Logout(
            [FromBody] BearerTokenRequest bearerTokenRequest)
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

                if (bearerTokenRequest.GrantType != BearerAuthenticationDefaults.RemoveGrantName)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        new ProblemDetails()
                        {
                            Detail = $"Invalid grant_type: {bearerTokenRequest.GrantType}",
                            StatusCode = HttpStatusCode.BadRequest,
                            Title = "Error"
                        });
                }

                if (!bearerTokenRequest.IsValid)
                {
                    return new ActionResult(HttpStatusCode.BadRequest,
                        bearerTokenRequest.GetValidationProblemDetails());
                }

                // Get User by UserName or fail
                var __user = await _identityManagerService.GetUserByUserNameAsync(bearerTokenRequest.UserName);
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

                // Remove Transient user tokens
                await _identityManagerService.RemoveUserTokens(__user, true);

                return new ActionResult(HttpStatusCode.NoContent);
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
