// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Models;
using System;
using System.Net;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// Standard result from Actions
    /// </summary>
    public class ActionResult : IActionResult
    {
        /// <inheritdoc/>
        public HttpStatusCode StatusCode { get; private set; }

        /// <inheritdoc/>
        public object ResultObject { get; private set; }

        /// <inheritdoc/>
        public Type ResultType
        {
            get
            {
                return ResultObject?.GetType() ?? typeof(String);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// Create a generic <see cref="ActionResult"/>
        /// </summary>
        /// <param name="statusCode">HTTP Status Code returned to Client</param>
        /// <param name="resultObject">Actual response object to be converted/formatted and returned to Client</param>
        public ActionResult(HttpStatusCode statusCode, object resultObject = null)
        {
            StatusCode = statusCode;
            ResultObject = resultObject;
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for an "OK" response.
        /// OK = 200.
        /// Used for:
        /// - GET = return an Entity header+body
        /// - HEAD = return an Entity header
        /// </summary>
        /// <param name="resultObject">Actual response object to be converted/formatted and returned to Client</param>
        /// <returns>An <see cref="ActionResult"/> for an "OK" response.</returns>
        public static ActionResult OK(object resultObject = null)
        {
            return new ActionResult(HttpStatusCode.OK, resultObject);
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for a "Created" response.
        /// Created = 201.
        /// Used for:
        /// - POST = An Entity has been created.
        /// Consider using <see cref="CreatedAt"/> instead.
        /// </summary>
        /// <param name="resultObject">Actual response object to be converted/formatted and returned to Client</param>
        /// <returns>An <see cref="ActionResult"/> for a "Created" response.</returns>
        public static ActionResult Created(object resultObject)
        {
            return new ActionResult(HttpStatusCode.Created, resultObject);
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for a "Created" response complete with simple HATEOS.
        /// </summary>
        /// <param name="resultObject">The Entity created which is to be returned to Client within a HATEOS response</param>
        /// <param name="links">HATEOS links for the Entity</param>
        /// <returns>A HateosActionResult object</returns>
        public static ActionResult CreatedAt(object resultObject, HateosResultObjectLinks links)
        {
            return GetHateosActionResult(HttpStatusCode.Created, resultObject, links);
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for an "Accepted" response.
        /// Accepted = 202.
        /// Used for:
        /// - POST = The request was Accepted but has not yet been processed.
        /// Consider sending a HATEOS variation that can be used to check progress.
        /// An example is to use <see cref="AcceptedWithStatusAt"/>.
        /// </summary>
        /// <param name="resultObject">Information about the request status converted/formatted and returned to Client</param>
        /// <returns>An <see cref="ActionResult"/> with information about the request to go as a response.</returns>
        public static ActionResult Accepted(object resultObject)
        {
            return new ActionResult(HttpStatusCode.Accepted, resultObject);
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for an "Accepted" response.
        /// Accepted = 202.
        /// Used for:
        /// - POST = The request was Accepted but has not yet been processed.
        /// </summary>
        /// <param name="resultObject">Information about the request status converted/formatted and returned to Client</param>
        /// <param name="links">HATEOS links for the Entity or Status of the Entity</param>
        /// <returns>A HateosActionResult object</returns>
        public static ActionResult AcceptedWithStatusAt(object resultObject, HateosResultObjectLinks links)
        {
            return GetHateosActionResult(HttpStatusCode.Accepted, resultObject, links);
        }

        /// <summary>
        /// Return an <see cref="ActionResult"/> for a "NoContent" response.
        /// NoContent = 204.
        /// Used for:
        /// - GET - where the Entity exists but no content is returnable
        /// - PUT - following an Update
        /// - PATCH - following an Update
        /// - DELETE - following a Deletion
        /// </summary>
        /// <returns>No Content</returns>
        public static ActionResult NoContent()
        {
            return new ActionResult(HttpStatusCode.NoContent, null);
        }

        /// <summary>
        /// TODO: put link into Location header
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public static HateosActionResult<object> MovedPermanently(HateosResultObjectLinks links)
        {
            return new HateosActionResult<object>(HttpStatusCode.MovedPermanently, null, links);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="resultObject"></param>
        /// <param name="links"></param>
        /// <returns></returns>
        public static ActionResult GetHateosActionResult(HttpStatusCode httpStatusCode, object resultObject, HateosResultObjectLinks links)
        {
            Type a1 = typeof(HateosActionResult<>);
            Type g1 = resultObject.GetType();
            Type actual = a1.MakeGenericType(new Type[] { g1 });
            IActionResult o =
                Activator.CreateInstance(actual, httpStatusCode, resultObject, links)
                as IActionResult;
            return o as ActionResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultObject"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static ActionResult OK(
            object resultObject, long totalRecordCount, long? pageSize = null, long? pageNumber = null)
        {
            return GetPagedActionResult(HttpStatusCode.OK, resultObject,
                totalRecordCount, pageSize, pageNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="resultObject"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static ActionResult GetPagedActionResult(HttpStatusCode httpStatusCode, object resultObject, long totalRecordCount, long? pageSize = null, long? pageNumber = null)
        {
            Type a1 = typeof(PagedActionResult<>);
            Type g1 = resultObject.GetType();
            Type actual = a1.MakeGenericType(new Type[] { g1 });
            IActionResult o =
                Activator.CreateInstance(actual, httpStatusCode, resultObject, totalRecordCount, pageSize, pageNumber)
                as IActionResult;
            return o as ActionResult;
        }

        #region Error Responses

        /// <summary>
        /// Use for:
        /// 400 = Bad Request
        /// 401 = Unauthorised ??
        /// 403 = Forbidden ??
        /// 404 = Not Found ??
        /// 405 = Method Not Allowed - no this will be handled before Action
        /// 409 = Conflict ??
        /// 411 = Length Required - can use FromHeader(Content-Length) or need to add a Header Requirement
        /// 412 = Procondition Failed ??
        /// 429 = Too Many Requests - will be handled before Action
        /// 500 = Internal Server Error
        /// 503 = Service Unavailable - will be handled before Action
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <returns></returns>
        public static ActionResult Error(ProblemDetails problemDetails)
        {
            return new ActionResult(problemDetails.StatusCode, problemDetails);
        }

        /// <summary>
        /// Returns a <see cref="ProblemDetails"/> <see cref="ActionResult"/> for a Bad Request.
        /// Bad Request = 400
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <returns></returns>
        public static ActionResult BadRequest(ProblemDetails problemDetails)
        {
            return new ActionResult(HttpStatusCode.BadRequest, problemDetails);
        }

        #endregion

    }
}
