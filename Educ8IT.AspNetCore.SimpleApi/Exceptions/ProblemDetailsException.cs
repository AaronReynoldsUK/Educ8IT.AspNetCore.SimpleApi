// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ProblemDetailsException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemDetails"></param>
        public ProblemDetailsException(ProblemDetails problemDetails)
            : base (problemDetails.Title)
        {
            this.ProblemDetails = problemDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        public ProblemDetails ProblemDetails
        {
            get;set;
        }
    }
}
