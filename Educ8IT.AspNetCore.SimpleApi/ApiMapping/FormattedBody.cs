// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using HttpMultipartParser;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.ApiMapping
{
    /// <summary>
    /// 
    /// </summary>
    public class FormattedBody
    {
        private readonly HttpRequest _request;

        /// <summary>
        /// 
        /// </summary>
        public FormattedBody() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public FormattedBody(HttpContext context)
        {
            _request = context?.Request ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Parse()
        {
            if (_request.ContentType == null)
            {
                return;
            }
            else if (_request.ContentType.StartsWith("multipart/form-data"))
            {
                HandleMultiPartForm();
            }
            else
            {
                // This method might be out-dated now there is a Pipeline reader
                if (_request.Body.CanRead)
                {
                    FormBody = await new StreamReader(_request.Body).ReadToEndAsync();
                }
                
                UploadedFiles = null;

                var __contentType = _request.GetTypedHeaders().ContentType;

                if (__contentType.IsSubsetOf(MediaTypeHeaderValue.Parse("text/xml"))
                    || __contentType.IsSubsetOf(MediaTypeHeaderValue.Parse("application/xml")))
                {
                    XmlFromBody = FormBody;
                    // read using correct encoding...
                    // currently using default method above but this might not be adequate for foreign character sets
                }
                else if (__contentType.IsSubsetOf(MediaTypeHeaderValue.Parse("application/json")))
                {
                    JsonFromBody = FormBody;
                }
                else if (__contentType.ToString() == "application/x-www-form-urlencoded")
                {
                    string[] pairs = FormBody.Split('&');

                    for (int x = 0; x < pairs.Length; x++)
                    {
                        string[] item = pairs[x].Split("=".ToCharArray(), 2, StringSplitOptions.None);
                        Form.Add(item[0], System.Web.HttpUtility.UrlDecode(item[1]));
                    }
                }
                else
                {
                    throw new NotImplementedException(
                        String.Format("ContentType {0} is not implemented.", _request.ContentType));
                }
            }
        }

        private async void HandleMultiPartForm()
        {
            FormBody = String.Empty;
            List<HttpUploadedFile> __uploadedFiles = new List<HttpUploadedFile>();

            var __requestId = Guid.NewGuid();
            var __smfdp = new StreamingMultipartFormDataParser(_request.Body);
            //Stream __syncStream = new MemoryStream();
            //await _request.Body.CopyToAsync(__syncStream);
            //__syncStream.Position = 0;
            //var __smfdp = new StreamingMultipartFormDataParser(__syncStream);

            __smfdp.ParameterHandler += parameter =>
            {
                Form.Add(parameter.Name, parameter.Data);
            };
            // TODO: check if this works
            // - the "partNumber" attribute is new so it may be that the parts are received out of sequence
            __smfdp.FileHandler += (name, fileName, type, disposition, buffer, bytes, partNumber) =>
            {
                var __uploadedFile = __uploadedFiles.FirstOrDefault(f => f.ParameterName == name);
                if (__uploadedFile == null)
                {
                    __uploadedFile = new HttpUploadedFile()
                    {
                        FileType = type,
                        OriginalFileName = fileName,
                        ParameterName = name
                    };
                    __uploadedFiles.Add(__uploadedFile);
                }
                __uploadedFile.WriteToFile(__requestId, buffer, bytes);
            };
            __smfdp.StreamClosedHandler += () =>
            {
                // do on close of streams
                foreach (var __uploadedFile in UploadedFiles)
                { }

            };
            //__smfdp.Run();
            await __smfdp.RunAsync();
            UploadedFiles = __uploadedFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection Form { get; private set; } = new NameValueCollection();

        /// <summary>
        /// 
        /// </summary>
        public List<HttpUploadedFile> UploadedFiles { get; private set; } = new List<HttpUploadedFile>();

        /// <summary>
        /// 
        /// </summary>
        public string FormBody { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string XmlFromBody { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string JsonFromBody { get; private set; }
    }
}
