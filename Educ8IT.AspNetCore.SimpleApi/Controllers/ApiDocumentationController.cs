using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.Documentation.Dtos;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.ThirdParty;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Controllers
{
    /// <summary>
    /// Provides documentation for the API
    /// </summary>
    [AllowAnonymous]
    [Version(1, 0)]
    public class ApiDocumentationController
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
        public ApiDocumentationController(
            HttpContext httpContext,
            ILogger<ApiDocumentationController> iLogger,
            IApiMapperService apiMapperService,
            IOptions<ApiMapperOptions> apiMapperOptions)
        {
            _iLogger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _apiMapperService = apiMapperService ?? throw new ArgumentNullException(nameof(apiMapperService));
            _apiMapperOptions = apiMapperOptions?.Value ?? throw new ArgumentNullException(nameof(apiMapperOptions));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [AllowedResponseContentType("application/educ8it.api.documentation+json")]
        public ActionResult GetApiDocumentation([FromHeader("apiVersion")] string apiVersion)
        {
            ApiVersion version = new ApiVersion(1, 0);

            if (apiVersion != null)
            {
                if (!ApiVersion.TryParse(apiVersion, out version))
                    throw new ArgumentException("Invalid apiVersion requested");
            }

            var __versionedControllers = _apiMapperService.ApiDescription.GetVersionedControllers(_apiMapperOptions);
            if (__versionedControllers != null)
            {
                if (__versionedControllers.ContainsKey(version))
                {
                    var __versionOfControllers = new Dictionary<ApiVersion, List<IApiControllerItem>>(
                    new[] {
                        __versionedControllers.FirstOrDefault(v => v.Key.Equals(version))
                    });
                    __versionedControllers = __versionOfControllers;
                }
                else throw new Exception("No such version");
            }
            else            
            {                
                var __nonVersionedControllers = _apiMapperService.ApiDescription.Controllers;
                __versionedControllers = new Dictionary<ApiVersion, List<IApiControllerItem>>();
                __versionedControllers.Add(version, __nonVersionedControllers);
            }

            return ActionResult.OK(_apiMapperService.ApiDescription.Controllers);

            var __documentationPaths = new DocumentationPaths()
            {
                RootDocumentationUriTemplate = "",
                ControllerDocumentationUriTemplate = "[CONTROLLER]",
                MethodDocumentationUriTemplate = "[CONTROLLER]/[METHOD]",
                TypeDocumentationUriTemplate = "type/[TYPE]"
            };
            var __versionedApi = new VersionedApiDto(__versionedControllers, __documentationPaths);
            
            //var __controllers = new List<ControllerDto>();
            //foreach ( var versionSet in __versionedControllers)
            //{
            //    foreach (var controller in versionSet.Value)
            //    {
            //        __controllers.Add(new ControllerDto(controller));
            //    }
            //}
            // Instead of returning a HTML doc,
            // let's return a JSON object that can be formatted
            // we register a custom formatter for
            // e.g. application/educ8it.api.documentation+json

            // we can return all top-level objects (for any API version)
            // and then filter, or take an API version param and send only those

            //IDictionary<string, List<string>> actions = new XmlSerializableDictionary<string, List<string>>();


            //_apiMapperService.Controllers.ForEach((controller) =>
            //{
            //    List<string> _actions = new List<string>();
            //    controller.Methods.ForEach((method) =>
            //    {
            //        _actions.Add(method.Name);
            //    });
            //    actions.Add(controller.Name, _actions);
            //});


            return ActionResult.OK(__versionedApi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet("{controllerName:alpha}")]
        [AllowAnonymous]
        [AllowedResponseContentType("application/educ8it.api.documentation+json")]
        //[ResponseType(HttpStatusCode.OK, typeof(new { }))]
        //[ResponseType(HttpStatusCode.NotFound, typeof(ProblemDetails))]
        public ActionResult GetApiDocumentationForController(
            [FromRoute("controllerName")] string ControllerName, string apiVersion)
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

            var __controllerDescription = _apiMapperService.ApiDescription.GetTypeDescription(__matchedController.Type);

            return ActionResult.OK(new ControllerDescriptionDto(__matchedController, __controllerDescription));
            //return ActionResult.OK($"name = {__controllerDescription.Name}, description = {__controllerDescription.Description}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        [HttpGet("{controllerName:alpha}/{methodName:alpha}")]
        [AllowAnonymous]
        [AllowedResponseContentType("application/educ8it.api.documentation+json")]
        [Version(1,0)]
        public ActionResult GetApiDocumentationForControllerMethod(
            [FromRoute("controllerName")] string ControllerName,
            [FromRoute("methodName")] string MethodName, 
            string apiVersion)
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

            var __methodDescription = _apiMapperService.ApiDescription.GetTypeDescription(__matchedMethod.Type);

            return ActionResult.OK(new MethodDescriptionDto(__matchedMethod, __methodDescription));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        [HttpGet("type/{typeName:alpha}")]
        [AllowAnonymous]
        [AllowedResponseContentType("application/educ8it.api.documentation+json")]
        [ResponseType(HttpStatusCode.OK, typeof(TypeDescriptionDto))]
        //[ResponseType(HttpStatusCode.OK, typeof(ControllerDescriptionDto))]
        //[ResponseType(HttpStatusCode.OK, typeof(MethodDescriptionDto))]
        public ActionResult GetApiDocumentationForType(
            [FromRoute("typeName")] string TypeName)
        {
            if (String.IsNullOrEmpty(TypeName))
                throw new ArgumentNullException(nameof(TypeName));

            Type __type = _apiMapperService.ApiDescription.GetTypeByName(TypeName);
            if (__type == null)
            {
                return new ActionResult(System.Net.HttpStatusCode.NotFound,
                    new ProblemDetails()
                    {
                        Detail = $"The type called {TypeName} was not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Title = "Not Found"
                    });
            }

            var __typeDescription = _apiMapperService.ApiDescription.GetTypeDescription(__type);

            return ActionResult.OK(new TypeDescriptionDto(__typeDescription));
        }
    }
}
