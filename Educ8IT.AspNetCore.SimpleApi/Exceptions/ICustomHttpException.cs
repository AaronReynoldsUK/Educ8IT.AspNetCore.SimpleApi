// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Exceptions
{
    /// <summary>
    /// A Custom Http error response
    /// </summary>
    public interface ICustomHttpException
    {
        public object ResponseObject { get; set; }

        public int StatusCode { get; set; }

        public List<Type> AdditionalTypesIncluded { get; }

        public bool ShouldSerializeClassName();

        public bool ShouldSerializeData();

        public bool ShouldSerializeInnerException();

        public bool ShouldSerializeHelpURL();

        public bool ShouldSerializeStackTrace();

        public bool ShouldSerializeStackTraceString();

        public bool ShouldSerializeHResult();

        public bool ShouldSerializeSource();

        public bool ShouldSerializeTargetSite();

        public bool ShouldSerializeRemoteStackTraceString();

        public bool ShouldSerializeRemoteStackIndex();

        public bool ShouldSerializeExceptionMethod();

        public bool ShouldSerializeWatsonBuckets();
    }
}
