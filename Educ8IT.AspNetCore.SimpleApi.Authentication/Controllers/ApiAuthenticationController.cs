using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController("ApiAuthentication", "auth", "v{version:apiVersion}/auth")]
    public class ApiAuthenticationController
    {
        private readonly ILogger _iLogger;
        private readonly HttpContext _httpContext;
        private readonly IApiMapperService _apiMapperService;
        private readonly ApiMapperOptions _apiMapperOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="iLogger"></param>
        /// <param name="apiMapperService"></param>
        /// <param name="apiMapperOptions"></param>
        public ApiAuthenticationController(
            HttpContext httpContext,
            ILogger<ApiAuthenticationController> iLogger,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
        }

        // POST token => grant type token request

        // POST mfa-challenge => request a challenge

        // PUT mfa-response

        // destroy


    }
}
