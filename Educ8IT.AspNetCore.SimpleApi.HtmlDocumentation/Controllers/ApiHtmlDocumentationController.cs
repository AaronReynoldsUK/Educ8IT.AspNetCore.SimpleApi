// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.DocumentationProviders;
using Educ8IT.AspNetCore.SimpleApi.Models;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Educ8IT.AspNetCore.SimpleApi.HtmlDocumentation.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    [ApiController("ApiHtmlDocumentation", "htmlDocs","v{version:apiVersion}/htmlDocs")]
    public class ApiHtmlDocumentationController
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
        public ApiHtmlDocumentationController(
            HttpContext httpContext,
            ILogger<ApiHtmlDocumentationController> iLogger,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
        }

        /// <summary>
        /// Gets the Controllers and Methods for this API
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [AllowedResponseContentType("text/vnd.educ8it.api.documentation+html")]
        public ActionResult GetApiDocumentation([FromHeader("apiVersion")] string apiVersion)
        {
            return ActionResult.OK(_apiMapperService.ApiDescription.Controllers);
        }

        /// <summary>
        /// Gets the documentation for a Controller and the Methods/Actions defined on it
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet("{controllerName:word}")]
        [AllowAnonymous]
        [AllowedResponseContentType("text/vnd.educ8it.api.documentation+html")]
        //[ResponseType(HttpStatusCode.OK, typeof(IApiControllerItem))]
        //[ResponseType(HttpStatusCode.NotFound, typeof(ProblemDetails))]
        public ActionResult GetApiDocumentationForController(
            [FromRoute("controllerName")] string ControllerName,
            [FromHeader("apiVersion")] string apiVersion)
        {
            if (String.IsNullOrEmpty(ControllerName))
                throw new ArgumentNullException(nameof(ControllerName));

            var __matchedController = _apiMapperService.Controllers.FirstOrDefault(c => c.Name == ControllerName);
            if (__matchedController == null)
            {
                return new ActionResult(System.Net.HttpStatusCode.NotFound,
                    new ProblemDetails()
                    {
                        Detail = $"The controller called {ControllerName} was not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "Not Found"
                    });
            }

            return ActionResult.OK(__matchedController);
        }

        /// <summary>
        /// Get documentation for a Method/Action
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet("{controllerName:word}/{methodName:word}")]
        [AllowAnonymous]
        [AllowedResponseContentType("text/vnd.educ8it.api.documentation+html")]
        //[ResponseType(HttpStatusCode.OK, typeof(IApiMethodItem))]
        //[ResponseType(HttpStatusCode.NotFound, typeof(ProblemDetails))]
        public ActionResult GetApiDocumentationForControllerMethod(
            [FromRoute("controllerName")] string ControllerName,
            [FromRoute("methodName")] string MethodName,
            [FromHeader("api-version")] string apiVersion)
        {
            if (String.IsNullOrEmpty(ControllerName))
                throw new ArgumentNullException(nameof(ControllerName));

            if (String.IsNullOrEmpty(MethodName))
                throw new ArgumentNullException(nameof(ControllerName));

            var __matchedController = _apiMapperService.Controllers.FirstOrDefault(c => c.Name == ControllerName);
            if (__matchedController == null)
            {
                return new ActionResult(System.Net.HttpStatusCode.NotFound,
                    new ProblemDetails()
                    {
                        Detail = $"The controller called {ControllerName} was not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "Not Found"
                    });
            }

            var __matchedMethod = __matchedController.Methods.FirstOrDefault(m => m.Name == MethodName);
            if (__matchedMethod == null)
            {
                return new ActionResult(System.Net.HttpStatusCode.NotFound,
                    new ProblemDetails()
                    {
                        Detail = $"The method called {MethodName} was not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "Not Found"
                    });
            }

            return ActionResult.OK(__matchedMethod);
        }

        /// <summary>
        /// Get documentation for a Type
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        [HttpGet("type/{type:word}", "type/type={type:word}")]
        [AllowAnonymous]
        [AllowedResponseContentType("text/vnd.educ8it.api.documentation+html")]
        //[ResponseType(HttpStatusCode.OK, typeof(IApiMethodItem))]
        //[ResponseType(HttpStatusCode.NotFound, typeof(ProblemDetails))]
        public ActionResult GetApiDocumentationForType(
            [FromRoute("type")] string TypeName)
        {
            if (String.IsNullOrEmpty(TypeName))
                throw new ArgumentNullException(nameof(TypeName));

            Type apiType = _apiMapperService.ApiDescription.GetTypeByName(TypeName, out IDocumentationProvider documentationProvider);
            
            if (apiType == null)
            {
                // Problem Details
                return new ActionResult(System.Net.HttpStatusCode.NotFound,
                    new ProblemDetails()
                    {
                        Detail = $"The type called {TypeName} was not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "Not Found"
                    });
            }
            else
            {
                var __typeDescription = _apiMapperService.ApiDescription.GetTypeDescription(apiType);

                if (__typeDescription == null)
                {
                    __typeDescription = _apiMapperService.ApiDescription.GenerateTypeDescription(apiType, documentationProvider);
                }
                    
                //TypeDescription typeDescription = new TypeDescriptions.SimpleTypeDescription(apiType);
                // TODO: (type).GetTypeDescription

                // get TypeDescription for Type and return it
                return ActionResult.OK(__typeDescription);
            }
        }
    }
}
