// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiDescriptions;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Common;
using Educ8IT.AspNetCore.SimpleApi.ContextExceptionHandlers;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Educ8IT.AspNetCore.SimpleApi.Services;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiMapperService : IApiMapperService
    {
        #region Private Fields

        private readonly ILogger<ApiMapperService> _logger;
        private readonly ApiMapperOptions _apiMapperOptions;
        private readonly IApiDescription _apiDescription;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="apiDescription"></param>
        /// <param name="apiMapperOptions"></param>
        public ApiMapperService(
            ILoggerFactory loggerFactory,
            IApiDescription apiDescription,
            IOptionsMonitor<ApiMapperOptions> apiMapperOptions)
        {
            _logger = loggerFactory?.CreateLogger<ApiMapperService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));

            _apiDescription = apiDescription ?? throw new ArgumentNullException(nameof(apiDescription));
            _apiMapperOptions = apiMapperOptions?.CurrentValue ?? throw new ArgumentNullException(nameof(apiMapperOptions));
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public ApiMapperOptions ApiMapperOptions
        {
            get { return _apiMapperOptions; }
        }

        /// <inheritdoc/>
        public IApiDescription ApiDescription
        {
            get
            {
                return _apiDescription;
            }
        }

        ///// <inheritdoc/>
        //public virtual IApiDescription ApiDescription
        //{
        //    get
        //    {
        //        if (_apiDescription == null)
        //            _apiDescription = new ApiDescription(this);

        //        return _apiDescription;
        //    }
        //}

        /// <inheritdoc/>
        public List<IApiControllerItem> Controllers
        {
            get
            {
                return ApiDescription?.Controllers;
            }
        }

        /// <inheritdoc/>
        public List<IApiMethodItem> Methods
        {
            get
            {
                return ApiDescription?.Methods;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiControllerItem"></param>
        /// <param name="apiMethodItem"></param>
        /// <returns></returns>
        public virtual RequestDelegate GetEndpointDelegateProxy(IApiControllerItem apiControllerItem, IApiMethodItem apiMethodItem)
        {
            if (apiControllerItem == null)
                throw new ArgumentNullException(nameof(apiControllerItem));

            if (apiMethodItem == null)
                throw new ArgumentNullException(nameof(apiMethodItem));

            if (apiMethodItem.MethodInfo == null)
                throw new InvalidOperationException("MethodInfo cannot be Null");

            return async (context) =>
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                EndpointContext endpointContext = new EndpointContext(context);

                // Wrap RequestExceptionHandlers here
                try
                {
                    await ProcessRequest(endpointContext);

                    if (!endpointContext.ShortCircuit)
                    {
                        // Select output formatter... and generate Response Object
                        ResponseObject responseObject = await endpointContext.FormatResponseAsync();

                        await endpointContext.HttpContext.ApplyResponseAsync(responseObject);
                    }

                }
                catch (CustomHttpException custom_http_ex)
                {
                    endpointContext.ShortCircuitWithException(custom_http_ex);
                }
                catch (Exception ex)
                {
                    endpointContext.ShortCircuitWithException(ex);
                }

                if (endpointContext.PipelineException != null)
                {
                    var __matchedExceptionHandler = ApiMapperOptions.ContextExceptionHandlers != null
                        ? ApiMapperOptions.ContextExceptionHandlers
                                .FirstOrDefault(eh => eh.IsHandled(endpointContext.PipelineException.GetType()))
                            ?? ApiMapperOptions.ContextExceptionHandlers
                                .FirstOrDefault(eh => eh.IsHandled(typeof(Exception)))
                        : null;

                    if (__matchedExceptionHandler != null)
                    {
                        await __matchedExceptionHandler.HandleExceptionAsync(endpointContext.PipelineException);
                    }
                    else
                    {
                        var __defaultHandler = new EndpointContextExceptionHandler();
                        await __defaultHandler.HandleExceptionAsync(endpointContext);
                    }
                }
            };
        }

        private async Task ProcessRequest(EndpointContext endpointContext)
        {
            //bool __shortCircuit = false;
            if (ApiMapperOptions.Filters != null)
            {
                foreach (var __preProcessor in ApiMapperOptions.Filters)
                {
                    //__shortCircuit 
                    endpointContext.ShortCircuit = await __preProcessor.PreExecution(endpointContext);

                    //if (__shortCircuit)
                    if (endpointContext.ShortCircuit)
                        break;
                }
            }

            //endpointContext.ShortCircuit = __shortCircuit;

            //if (__shortCircuit)
            if (endpointContext.ShortCircuit)
                return;

            endpointContext.CheckRequest();

            endpointContext.UpdateResponseContentType();

            await endpointContext.ParseBodyAsync();

            var __methodArguments = await endpointContext.GetMethodArgumentsAsync();

            var __constructorInstance = GetContructorInstance(endpointContext);

            await InvokeMethod(endpointContext, __constructorInstance, __methodArguments);

            CheckResponseType(endpointContext);

            //if (!__shortCircuit)
            if (!endpointContext.ShortCircuit)
            {
                foreach (var __postProcessor in ApiMapperOptions.Filters.Reverse<Filters.IFilter>())
                {
                    //__shortCircuit 
                    endpointContext.ShortCircuit = await __postProcessor.PostExecution(endpointContext);

                    //if (__shortCircuit)
                    if (endpointContext.ShortCircuit)
                        break;
                }
            }

            //endpointContext.ShortCircuit = __shortCircuit;
        }

        private object GetContructorInstance(EndpointContext endpointContext)
        {
            if (endpointContext.ShortCircuit)
                return null;

            // Get Controller class
            var __controller = endpointContext.ApiMethodItem.MethodInfo.DeclaringType;  //apiControllerItem.Type;
            var __constructorInfo = __controller.GetConstructors()?.FirstOrDefault() ?? null;
            if (__constructorInfo == null)
            {
                endpointContext.ShortCircuitWithException(
                    new CustomHttpException(
                        "Invalid API specification", 
                        HttpStatusCode.InternalServerError));
                return null;
            }

            // Do dependency injection on the contructor
            var __constructorParameters = __constructorInfo.GetParameters();
            var __constructorServices = new List<object>();
            foreach (var __constructorParameter in __constructorParameters)
            {
                object __constructorService = null;

                if (__constructorParameter.ParameterType == typeof(EndpointContext))
                    __constructorService = endpointContext;
                else if (__constructorParameter.ParameterType == typeof(HttpContext))
                    __constructorService = endpointContext.HttpContext;
                else
                {
                    __constructorService = endpointContext.HttpContext.RequestServices
                        .GetRequiredService(__constructorParameter.ParameterType);
                }

                if (__constructorService == null)
                {
                    ApiDescription.Controllers.RemoveAll(c => c.ControllerType == __controller);
                    ApiDescription.Reset();

                    endpointContext.ShortCircuitWithException(
                        new CustomHttpException(
                            "Unable to bind services for this Endpoint",
                            HttpStatusCode.InternalServerError));
                    return null;
                }

                __constructorServices.Add(__constructorService);
            }

            try
            {
                return Activator.CreateInstance(__controller, __constructorServices.ToArray());
            }
            catch (Exception ex)
            {
                endpointContext.ShortCircuitWithException(ex);
            }
            return null;
        }

        private async Task InvokeMethod(EndpointContext endpointContext, object classInstance, List<object> methodArguments)
        {
            if (endpointContext.ShortCircuit)
                return;

            if (classInstance == null)
            {
                endpointContext.ShortCircuitWithException(
                    new CustomHttpException(
                        "Unable to unbox constructor for this Endpoint",
                        HttpStatusCode.InternalServerError));
                return;
            }

            // Call endpoint
            object __methodResponse = null;
            
            // Get method return type
            var __methodReturnType = endpointContext.ApiMethodItem.MethodInfo.ReturnType;

            try
            {
                if (endpointContext.ApiMethodItem.IsAsyncMethod)
                {
                    object __asyncInvoke = endpointContext.ApiMethodItem.MethodInfo
                        .Invoke(classInstance, methodArguments.ToArray());

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
                    object __syncInvoke = endpointContext.ApiMethodItem.MethodInfo
                        .Invoke(classInstance, methodArguments.ToArray());

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
                IActionResult actionResult = null;
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

                endpointContext.ActionResult = actionResult;

                //Type __actualReturnType = null;
                //__actualReturnType = actionResult.ResultType;
            }
            catch (Exception ex)
            {
                endpointContext.ShortCircuitWithException(ex);
            }
        }

        private void CheckResponseType(EndpointContext endpointContext)
        {
            if (endpointContext.ShortCircuit)
                return;

            if (endpointContext.ActionResult == null)
                return;

            // Check for matching Response Type
            var __responseTypeMatchCount = endpointContext.ApiMethodItem.ResponseTypes
                .Count(e => e.Value == endpointContext.ActionResult.ResultType);

            var __responseTypeMatch = endpointContext.ApiMethodItem.ResponseTypes
                .FirstOrDefault(e => e.Value == endpointContext.ActionResult.ResultType);

            var __statusCode = (int)endpointContext.ActionResult.StatusCode;
            var __expectedType = endpointContext.ApiMethodItem.ResponseTypes.ContainsKey(__statusCode)
                ? endpointContext.ApiMethodItem.ResponseTypes.FirstOrDefault(t => t.Key == __statusCode).Value
                : null;
            var __responseTypeSent = endpointContext.ActionResult.ResultType.GetReadableTypeNameTextPlain();
            var __responseTypeExpected = __expectedType?.GetReadableTypeNameTextPlain() ?? "U/K";

            if (__responseTypeMatchCount == 0 && endpointContext.ApiMethodItem.ResponseTypes.Count > 0)
            {
                endpointContext.ShortCircuitWithException(
                    new CustomHttpException(
                        $"Unrecognised Response Type in API: {__responseTypeSent}\nExpected Type: {__responseTypeExpected}",
                        HttpStatusCode.InternalServerError));
                return;
            }
        }

        #endregion
    }
}
