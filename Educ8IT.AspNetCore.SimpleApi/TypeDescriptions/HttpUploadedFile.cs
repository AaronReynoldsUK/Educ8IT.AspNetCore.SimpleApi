// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.TypeDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class HttpUploadedFile
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpUploadedFile() { }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int64 FileLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="buffer"></param>
        /// <param name="byteLength"></param>
        public void WriteToFile(Guid requestId, byte[] buffer, int byteLength)
        {
            try
            {
                if (String.IsNullOrEmpty(ServerFileName))
                {
                    string __applicationDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CRM Server");
                    string __fileUploadsFolder = System.IO.Path.Combine(__applicationDataPath, "uploads");
                    string __fileUploadPath = System.IO.Path.Combine(__fileUploadsFolder, requestId.ToString());

                    if (!System.IO.Directory.Exists(__fileUploadPath))
                        System.IO.Directory.CreateDirectory(__fileUploadPath);

                    OriginalFileName = FileOperations.SanitiseFileName(OriginalFileName);

                    if (String.IsNullOrEmpty(OriginalFileName))
                        throw new Exception("Invalid filename");

                    var __combinedPath = System.IO.Path.Combine(__fileUploadPath, OriginalFileName);

                    if (File.Exists(__combinedPath))
                        File.Delete(__combinedPath);

                    ServerFileName = __combinedPath;
                }

                using (var __fs = new FileStream(ServerFileName, FileMode.Append))
                {
                    __fs.Write(buffer, 0, byteLength);
                }
                FileLength += byteLength;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
