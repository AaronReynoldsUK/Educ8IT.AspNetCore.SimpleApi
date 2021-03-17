// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ApiMapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class MediaTypeHeaderValueExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaTypeHeaderValue"></param>
        /// <param name="mediaTypeHeaderValueForComparision"></param>
        /// <returns></returns>
        public static bool MatchesTypeAndSuffixOrSubType(
            this MediaTypeHeaderValue mediaTypeHeaderValue,
            string mediaTypeHeaderValueForComparision
            )
        {
            return mediaTypeHeaderValue.MatchesTypeAndSuffixOrSubType(MediaTypeHeaderValue.Parse(mediaTypeHeaderValueForComparision));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaTypeHeaderValue"></param>
        /// <param name="mediaTypeHeaderValueForComparision"></param>
        /// <returns></returns>
        public static bool MatchesTypeAndSuffixOrSubType(
            this MediaTypeHeaderValue mediaTypeHeaderValue, 
            MediaTypeHeaderValue mediaTypeHeaderValueForComparision)
        {
            if (mediaTypeHeaderValue.Type != mediaTypeHeaderValueForComparision.Type)
                return false;

            var mediaTypeHeaderValueSubTypeOrSuffix = mediaTypeHeaderValue.Suffix.HasValue
                ? mediaTypeHeaderValue.Suffix.ToString()
                : mediaTypeHeaderValue.SubType.ToString();

            if (String.IsNullOrEmpty(mediaTypeHeaderValueSubTypeOrSuffix))
                return false;

            var mediaTypeHeaderValueForComparisionSubTypeOrSuffix = mediaTypeHeaderValueForComparision.Suffix.HasValue
                ? mediaTypeHeaderValueForComparision.Suffix.ToString()
                : mediaTypeHeaderValueForComparision.SubType.ToString();

            if (String.IsNullOrEmpty(mediaTypeHeaderValueForComparisionSubTypeOrSuffix))
                return false;

            return mediaTypeHeaderValueSubTypeOrSuffix == mediaTypeHeaderValueForComparisionSubTypeOrSuffix;
        }
    }
}
