// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Educ8IT.AspNetCore.SimpleApi.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Models
{
    /// <summary>
    /// Implementation of https://tools.ietf.org/html/rfc7807
    /// Response type should be application/problem+json or application/problem+xml
    /// Sub-classes can add additional extensions for e.g. validation information
    /// </summary>
    [Serializable()]
    [XmlRoot("problemDetails")]
    public class ProblemDetails
    {
        /// <summary>
        /// A URI reference [RFC3986] that identifies the problem type. 
        /// This specification encourages that, when dereferenced, 
        ///   it provide human-readable documentation for the 
        ///   problem type (e.g., using HTML [W3C.REC-html5-20141028]).  
        ///   When this member is not present, its value is assumed to be "about:blank".
        /// </summary>
        [JsonPropertyName("type")]
        [XmlElement("type")]
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type.  
        /// It SHOULD NOT change from occurrence to occurrence of the problem, 
        ///   except for purposes of localization 
        ///   (e.g., using proactive content negotiation; see[RFC7231], Section 3.4).
        /// </summary>
        [JsonPropertyName("title")]
        [XmlElement("title")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The HTTP status code ([RFC7231], Section 6) 
        ///   generated by the origin server for this occurrence of the problem.
        /// </summary>
        [JsonPropertyName("status")]
        [XmlElement("status")]
        [Required]
        public int Status
        {
            get
            {
                return (int)StatusCode;
            }
            set
            {
                if (Enum.TryParse<HttpStatusCode>(value.ToString(), out HttpStatusCode httpStatusCode))
                    StatusCode = httpStatusCode;
                else
                    StatusCode = HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public HttpStatusCode StatusCode
        {
            get; set;
        }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        [JsonPropertyName("detail")]
        [XmlElement("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the problem.  
        /// It may or may not yield further information if dereferenced.
        /// </summary>
        [JsonPropertyName("instance")]
        [XmlElement("instance")]
        public string Instance { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[XmlElement("extensions")]
        //[JsonExtensionData]
        //[JsonPropertyName("extensions")]
        //public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>();

        ///// <summary>
        ///// 
        ///// </summary>
        //[JsonIgnore]
        //[XmlIgnore()]
        //public IDictionary<string, object> JsonExtensions
        //{
        //    get
        //    {
        //        var _dictionary = new Dictionary<string, object>();

        //        foreach (var __entry in Extensions)
        //        {
        //            _dictionary.Add(__entry.Key, __entry.Value);
        //        }

        //        return _dictionary;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("validation")]
        [XmlArray("validation")]
        [XmlArrayItem("validationItem")]
        public List<ProblemDetailsExtension> Validation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("extensions")]
        [XmlArray("extensions")]
        [XmlArrayItem("extensionsItem")]
        public List<ProblemDetailsExtension> Extensions { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    //[XmlRoot("extensionsItem")]
    //[XmlArrayItem("extensionsItem")]
    public class ProblemDetailsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("key")]
        [XmlElement("key")]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("items")]
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<string> Items { get; set; }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[Serializable]
    //[XmlRoot("listOfProblemDetailsObjectItem")]
    //public class ListOfProblemDetailsExtensionObjectItem : List<ProblemDetailsExtensionObjectItem>
    //{

    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //public interface IProblemDetailsExtensionObjectItem
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [JsonPropertyName("key")]
    //    public string Key { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [JsonPropertyName("values")]
    //    public List<string> Values { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    public void AddItem(string value)
    //    {
    //        if (String.IsNullOrEmpty(value))
    //            return;
            
    //        if (this.Values == null)
    //            this.Values = new List<string>();

    //        this.Values.Add(value);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="values"></param>
    //    public void AddItems(List<string> values)
    //    {
    //        if (values == null || values.Count == 0)
    //            return;

    //        if (this.Values == null)
    //            this.Values = new List<string>();

    //        this.Values.AddRange(values);
    //    }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //[Serializable]
    //[XmlRoot("problemDetailsObjectItem")]
    //public class ProblemDetailsExtensionObjectItem : IProblemDetailsExtensionObjectItem
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public ProblemDetailsExtensionObjectItem() { }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    public ProblemDetailsExtensionObjectItem(string key, string value)
    //    {
    //        this.Key = key;
    //        this.AddItem(value);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="values"></param>
    //    public ProblemDetailsExtensionObjectItem(string key, List<string> values) {
    //        this.Key = key;
    //        this.AddItems(values);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [JsonPropertyName("key")]
    //    [XmlElement("key")]
    //    public string Key { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [JsonPropertyName("values")]
    //    [XmlElement("values")]
    //    public List<string> Values { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="value"></param>
    //    public void AddItem(string value)
    //    {
    //        if (String.IsNullOrEmpty(value))
    //            return;

    //        if (this.Values == null)
    //            this.Values = new List<string>();

    //        this.Values.Add(value);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="values"></param>
    //    public void AddItems(List<string> values)
    //    {
    //        if (values == null || values.Count == 0)
    //            return;

    //        if (this.Values == null)
    //            this.Values = new List<string>();

    //        this.Values.AddRange(values);
    //    }
    //}
}
