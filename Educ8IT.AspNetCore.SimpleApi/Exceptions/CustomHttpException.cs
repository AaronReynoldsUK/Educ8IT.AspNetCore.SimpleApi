// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Exceptions
{
    /// <summary>
    /// A Custom Http error response
    /// </summary>
    [Serializable]
    [XmlRoot("httpErrorResponse")]
    //[XmlInclude(typeof(TokenSystemResponse_Unsuccessful))]
    [DataContract]
    public class CustomHttpException : Exception, ICustomHttpException
    {
        /// <summary>
        /// Optional included data, e.g. a token failure response
        /// </summary>
        [DataMember]
        public object ResponseObject { get; set; }

        /// <summary>
        /// HTTP Status code
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public CustomHttpException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public CustomHttpException(string message, int statusCode)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        public CustomHttpException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.StatusCode = (int)statusCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <param name="statusCode"></param>
        public CustomHttpException(string message, Exception inner, int statusCode)
            : base(message, inner)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Serialisation Helper
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public List<Type> AdditionalTypesIncluded
        {
            get
            {
                List<Type> types = new List<Type>();

                if (ResponseObject != null)
                    types.Add(ResponseObject.GetType());

                return types;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassName() { return false; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeData() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeInnerException() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeHelpURL() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeStackTrace() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeStackTraceString() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeHResult() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSource() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTargetSite() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemoteStackTraceString() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemoteStackIndex() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeExceptionMethod() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeWatsonBuckets() { return false; }
    }
}
