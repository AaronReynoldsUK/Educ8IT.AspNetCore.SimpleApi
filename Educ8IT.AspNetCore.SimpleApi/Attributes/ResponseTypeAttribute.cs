// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ResponseTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public int HttpResponseCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode HttpStatusCode
        {
            get { return (HttpStatusCode)HttpResponseCode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ResponseType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponseCode"></param>
        /// <param name="responseType"></param>
        public ResponseTypeAttribute(int httpResponseCode, Type responseType)
        {
            HttpResponseCode = httpResponseCode;
            ResponseType = responseType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponseCode"></param>
        /// <param name="responseType"></param>
        public ResponseTypeAttribute(HttpStatusCode httpResponseCode, Type responseType)
        {
            HttpResponseCode = (int)httpResponseCode;
            ResponseType = responseType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponseCode"></param>
        public ResponseTypeAttribute(HttpStatusCode httpResponseCode)
        {
            HttpResponseCode = (int)httpResponseCode;
        }
    }
}
