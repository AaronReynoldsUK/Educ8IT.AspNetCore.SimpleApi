using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Authorisation.TypeDescriptions;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Services;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authorisation
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationAwareApiMapperService : ApiMapperService
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="apiDescription"></param>
        /// <param name="apiMapperOptions"></param>
        public AuthorisationAwareApiMapperService(
            ILoggerFactory loggerFactory,
            IApiDescription apiDescription,
            IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
            : base(loggerFactory, apiDescription, apiMapperOptions)
        { }

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiControllerItem"></param>
        /// <param name="apiMethodItem"></param>
        /// <returns></returns>
        public override RequestDelegate GetEndpointDelegateProxy(IApiControllerItem apiControllerItem, IApiMethodItem apiMethodItem)
        {
            if (apiControllerItem == null)
                throw new ArgumentNullException(nameof(apiControllerItem));

            if (apiMethodItem == null)
                throw new ArgumentNullException(nameof(apiMethodItem));

            if (apiMethodItem.MethodInfo == null)
                throw new InvalidOperationException("MethodInfo cannot be Null");

            //if (_apiMapperOptions)

            return async (context) =>
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                try
                {
                    
                    var __endpoint = context.GetEndpoint();
                    var __methodItem = __endpoint.Metadata.GetMetadata<ApiMethodItem>();
                    var __routeData = context.GetRouteData();

                    // Authorisation
                    if (apiMethodItem is AuthorisationAwareApiMethodItem authorisationAwareApiMethodItem)
                    {
                        if (authorisationAwareApiMethodItem.AuthoriseAttribute != null)
                        {
                            var authCheck = await context.AuthorisationCheck(authorisationAwareApiMethodItem);

                            if (!authCheck.Succeeded)
                            {
                                if (authCheck.Failure.FailCalled)
                                {
                                    // Log that Fail was called
                                }
                                else
                                {
                                    foreach (var __failedRequirement in authCheck.Failure.FailedRequirements)
                                    {
                                        // Log which requirements failed...
                                        //__failedRequirement.GetType().Name;
                                    }
                                }
                                
                                await context.ForbidAsync();
                                throw new CustomHttpException("Not Authorised", HttpStatusCode.Forbidden);
                            }
                        }
                    }

                    // Determine if HTTPMethod matches
                    if (!__methodItem.IsAllowedHttpMethod(context.Request.Method))
                        throw new CustomHttpException("HTTP Method not allowed", HttpStatusCode.MethodNotAllowed);

                    // Determine if Request Content Types matches
                    if (!__methodItem.IsRequestContentTypeMatch(context.Request.ContentType))
                        throw new CustomHttpException("Request ContentType not allowed", HttpStatusCode.UnprocessableEntity);

                    // Determine if Response Content Type matches
                    if (!__methodItem.IsResponseContentTypeMatch(context.Request.GetTypedHeaders().Accept))
                        throw new CustomHttpException("Response ContentType not allowed (Check Accept header)", HttpStatusCode.UnprocessableEntity);

                    // Determine actual response type
                    // - can actually remove previous call as this will return NULL if no options
                    var __responseType = __methodItem.GetPrimaryResponseContentType(context.Request.GetTypedHeaders().Accept);

                    if (__responseType == null)
                        throw new CustomHttpException("No ContentType specified for Action. This is an API fault", HttpStatusCode.InternalServerError);


                    // Get parsed Body
                    FormattedBody __formattedBody = new FormattedBody(context);
                    await __formattedBody.Parse();

                    // Convert/Pass parameters (so we don't waste time on this unless it matches all previous)
                    var __methodArguments = await __methodItem.GetMethodArguments(context, __formattedBody);

                    // Determine if Arguments match - need to check this works accurately
                    if (__methodItem.MethodParameters.Count != __methodArguments.Count)
                        throw new CustomHttpException("Invalid number of Arguments supplied", HttpStatusCode.BadRequest);


                    // Get Controller class
                    var __controller = apiMethodItem.MethodInfo.DeclaringType;  //apiControllerItem.Type;
                    var __constructorInfo = __controller.GetConstructors()?.FirstOrDefault() ?? null;
                    if (__constructorInfo == null)
                        throw new CustomHttpException("Invalid API specification", HttpStatusCode.InternalServerError);


                    // Do DI for Controller class
                    var __constructorParameters = __constructorInfo.GetParameters();
                    var __constructorServices = new List<object>();
                    foreach (var __constructorParameter in __constructorParameters)
                    {
                        object __constructorService = null;

                        if (__constructorParameter.ParameterType == typeof(HttpContext))
                            __constructorService = context;
                        else
                        {
                            __constructorService = context.RequestServices.GetRequiredService(__constructorParameter.ParameterType);
                        }

                        // TODO: remove this Controller from the Endpoints so it doesn't get called again
                        if (__constructorService == null)
                        {
                            ApiDescription.Controllers.RemoveAll(c => c.ControllerType == __controller);
                            ApiDescription.Reset();

                            throw new CustomHttpException("Unable to bind services for this Endpoint", HttpStatusCode.InternalServerError);
                        }

                        __constructorServices.Add(__constructorService);
                    }


                    // Get method return type
                    var __methodReturnType = __methodItem.MethodInfo.ReturnType;

                    // Generate a class instance - maybe we should cache these ??
                    // ... and pass in the DI services
                    dynamic __classInstance = Activator.CreateInstance(__controller, __constructorServices.ToArray());


                    // Call endpoint
                    object __methodResponse = null;
                    Type __actualReturnType = null;
                    IActionResult actionResult = null;

                    if (__methodItem.IsAsyncMethod)
                    {
                        object __asyncInvoke = __methodItem.MethodInfo.Invoke(__classInstance, __methodArguments.ToArray());

                        if (__methodReturnType.IsGenericType && __methodReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            if (__asyncInvoke is Task __asyncTask)
                            {
                                await __asyncTask;

                                switch (__asyncTask.Status)
                                {
                                    case TaskStatus.Faulted:
                                        throw __asyncTask?.Exception ?? new Exception("Unknown fault on API endpoint");
                                    case TaskStatus.Canceled:
                                        throw new Exception("Action was cancelled on API endpoint");
                                }

                                __methodResponse = __asyncTask.GetType().GetProperty("Result").GetValue(__asyncTask);
                            }
                        }
                        else if (__methodReturnType == typeof(Task))
                        {
                            // shouldn't happen because this a VOID response
                            // these are filtered out in the ApiMapping process
                        }
                    }
                    else
                    {
                        object __syncInvoke = __methodItem.MethodInfo.Invoke(__classInstance, __methodArguments.ToArray());

                        if (__methodReturnType.IsGenericType && __methodReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            if (__syncInvoke is Task __syncTask)
                            {
                                switch (__syncTask.Status)
                                {
                                    case TaskStatus.Faulted:
                                        throw __syncTask?.Exception ?? new Exception("Unknown fault on API endpoint");
                                    case TaskStatus.Canceled:
                                        throw new Exception("Action was cancelled on API endpoint");
                                }

                                __methodResponse = __syncTask.GetType().GetProperty("Result").GetValue(__syncTask);
                            }
                        }
                        else if (__methodReturnType == typeof(Task))
                        {
                            // shouldn't happen because this a VOID response
                            // these are filtered out in the ApiMapping process
                        }
                        else
                        {
                            __methodResponse = __syncInvoke;
                        }
                    }


                    // Convert method response to an IActionResult
                    if (typeof(IActionResult).GetTypeInfo().IsAssignableFrom(__methodResponse.GetType()))
                    {
                        actionResult = (IActionResult)__methodResponse;
                    }
                    else if (__methodResponse.GetType().GetInterfaces().Count(i => i == typeof(IActionResult)) != 0)
                    {
                        actionResult = (IActionResult)__methodResponse;
                    }
                    else if (__methodReturnType.IsAssignableFrom(typeof(IActionResult)))
                    {
                        actionResult = (IActionResult)__methodResponse;
                    }
                    else
                    {
                        actionResult = new ActionResult(HttpStatusCode.OK, __methodResponse);
                    }
                    __actualReturnType = actionResult.ResultType;


                    if (__actualReturnType != null)
                    {
                        // Add default Response Type if none provided
                        if (__methodItem.ResponseTypes.Count == 0)
                            __methodItem.ResponseTypes.Add((int)HttpStatusCode.OK, __actualReturnType);

                        // Check for matching Response Type
                        var __responseTypeMatchCount = __methodItem.ResponseTypes.Count(e => e.Value == __actualReturnType);
                        var __responseTypeMatch = __methodItem.ResponseTypes.FirstOrDefault(e => e.Value == __actualReturnType);
                        if (__responseTypeMatchCount == 0)
                            throw new CustomHttpException("Unrecognised Response Type in API", HttpStatusCode.InternalServerError);
                    }

                    // Select output formatter... and generate Response Object
                    ResponseObject responseObject = await context.FormatResponse(actionResult, __responseType);

                    await context.ApplyResponse(responseObject);
                }
                catch (CustomHttpException custom_http_ex)
                {
                    if (context.Response.HasStarted)
                        return;

                    await context.ErrorWriter(custom_http_ex);
                }
                catch (Exception ex)
                {
                    if (context.Response.HasStarted)
                        return;

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    while (ex != null)
                    {
                        await context.Response.WriteAsync(ex.Message);
                        await context.Response.WriteAsync(Environment.NewLine);
                        ex = ex.InnerException;
                    }

                    //var __pd = new ProblemDetails()
                    //{
                    //    Detail = ex.Message,
                    //    Status = (int)HttpStatusCode.InternalServerError
                    //};


                }
            };
        }

        #endregion
    }
}
