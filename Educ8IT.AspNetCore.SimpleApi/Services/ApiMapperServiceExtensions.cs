// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ActionResults;
using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Educ8IT.AspNetCore.SimpleApi.Exceptions;
using Educ8IT.AspNetCore.SimpleApi.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiMapperServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async static Task ErrorWriter(this HttpContext context, Exception exception)
        {
            if (exception is CustomHttpException customHttpException)
            {
                string __message = customHttpException.Message;
                context.Response.StatusCode = customHttpException.StatusCode;
                context.Response.ContentType = "text/plain";
                context.Response.ContentLength = __message.Length;

                await context.Response.WriteAsync(__message);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                while (exception != null)
                {
                    // TODO: This should be allowed on non-production only
                    await context.Response.WriteAsync(exception.Message);
                    await context.Response.WriteAsync(Environment.NewLine);
                    exception = exception.InnerException;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionResult"></param>
        /// <param name="mediaTypeHeaderValue"></param>
        /// <returns></returns>
        public static async Task<ResponseObject> FormatResponseAsync(this HttpContext context, 
            IActionResult actionResult, 
            MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            ResponseObject responseObject = new ResponseObject(context, actionResult);

            if (responseObject.ActionResult.ResultType == null)
                return responseObject;

            var __mapperIOptions = context.RequestServices.GetRequiredService<IOptions<ApiMapperOptions>>();
            var __mapperOptions = __mapperIOptions.Value;
            bool __formatMatched = false;

            foreach (IOutputFormatter outputFormatter in __mapperOptions.OutputFormatters)
            {
                // Check for straightforward match e.g. application/json
                if (outputFormatter.SupportedMediaTypeValue.MediaType != mediaTypeHeaderValue.MediaType)
                    continue;

                __formatMatched = true;

                if (outputFormatter.HandlesAsyncFormatting)
                    responseObject = await outputFormatter.FormatResponseAsync(responseObject);
                else
                {
                    responseObject = outputFormatter.FormatResponse(responseObject);
                }

                if (__formatMatched)
                    break;
            }

            if (!__formatMatched)
            {
                foreach (IOutputFormatter outputFormatter in __mapperOptions.OutputFormatters)
                {
                    if (outputFormatter.SupportedMediaTypeValue.MatchesTypeAndSuffixOrSubType(mediaTypeHeaderValue))
                    {
                        __formatMatched = true;

                        if (outputFormatter.HandlesAsyncFormatting)
                            responseObject = await outputFormatter.FormatResponseAsync(responseObject);
                        else
                        {
                            responseObject = outputFormatter.FormatResponse(responseObject);
                        }
                    }

                    if (__formatMatched)
                        break;
                }
            }

            if (!__formatMatched)
                throw new CustomHttpException("Unable to find matching Output Formatter", System.Net.HttpStatusCode.InternalServerError);

            return responseObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseObject"></param>
        /// <returns></returns>
        public async static Task ApplyResponseAsync(this HttpContext context, ResponseObject responseObject)
        {
            if (!String.IsNullOrEmpty(responseObject.ContentType))
            {
                context.Response.ContentLength = responseObject.ContentLength;
                context.Response.ContentType = responseObject.ContentType;
            }
            
            context.Response.StatusCode = (int)responseObject.ActionResult.StatusCode;

            if (responseObject.Headers != null)
            {
                foreach(var __header in responseObject.Headers)
                {
                    if (context.Response.Headers.ContainsKey(__header.Key))
                        context.Response.Headers[__header.Key] = __header.Value;
                    else
                        context.Response.Headers.Add(__header.Key, __header.Value);
                }
            }

            if (responseObject.FormattedResponseContent is string __responseString)
            {
                await context.Response.WriteAsync(__responseString ?? String.Empty);
            }
            else if (responseObject.FormattedResponseContent is Stream __responseStream)
            {
                if (__responseStream.CanSeek)
                    __responseStream.Position = 0;

                await context.Response.StartAsync();
                long __responseStreamLength = __responseStream.Length;

                byte[] __outputBuffer = new byte[4096];
                int __bytesRead = -1;
                while (__bytesRead != 0)
                {
                    __bytesRead = await __responseStream.ReadAsync(__outputBuffer, 0, __outputBuffer.Length);

                    if (__bytesRead > 0)
                        await context.Response.Body.WriteAsync(__outputBuffer, 0, __bytesRead);
                }
            }
        }

    }
}
