// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// Common File Operations
    /// </summary>
    public class FileOperations
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SanitiseFileName(string fileName)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            return String.Join("_", fileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}
