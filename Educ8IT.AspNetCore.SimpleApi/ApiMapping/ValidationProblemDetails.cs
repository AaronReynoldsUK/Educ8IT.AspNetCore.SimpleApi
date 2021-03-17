// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Educ8IT.AspNetCore.SimpleApi.ApiMapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProblemDetailsValidationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void AddValidationItem(this ProblemDetails problemDetails, string key, params string[] values)
        {
            if (problemDetails.Validation == null)
                problemDetails.Validation = new List<ProblemDetailsExtension>();

            if (!problemDetails.Validation.Exists(item => item.Key == key))
            {
                problemDetails.Validation.Add(new ProblemDetailsExtension()
                {
                    Key = key,
                    Items = values.ToList()
                });
            }
            else
            {
                problemDetails.Validation.FirstOrDefault(k => k.Key == key)
                    .Items.AddRange(values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void AddItem(this ProblemDetails problemDetails, string key, params string[] values)
        {
            if (problemDetails == null)
                return;

            if (problemDetails.Extensions == null)
                problemDetails.Extensions = new List<ProblemDetailsExtension>();

            if (!problemDetails.Extensions.Exists(item => item.Key == key))
            {
                problemDetails.Extensions.Add(new ProblemDetailsExtension()
                {
                    Key = key,
                    Items = values.ToList()
                });
            }
            else
            {
                problemDetails.Extensions.FirstOrDefault(k => k.Key == key)
                    .Items.AddRange(values);
            }
        }
    }

}
