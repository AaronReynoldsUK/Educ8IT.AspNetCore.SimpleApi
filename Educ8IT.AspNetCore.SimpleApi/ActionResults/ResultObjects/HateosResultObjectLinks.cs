// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// 
    /// </summary>
    public class HateosResultObjectLinks
    {
        /// <summary>
        /// The Uri to GET this Entity
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("get")]
        [Newtonsoft.Json.JsonProperty("get", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [XmlElement("get")]
        public string Get { get; set; }

        /// <summary>
        /// The Uri to UPDATE this Entity
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("update")]
        [Newtonsoft.Json.JsonProperty("update", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [XmlElement("update")]
        public string Update { get; set; }

        /// <summary>
        /// The Uri to PATCH this Entity
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("patch")]
        [Newtonsoft.Json.JsonProperty("patch", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [XmlElement("patch")]
        public string Patch { get; set; }

        /// <summary>
        /// The Uri to DELETE this Entity
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("delete")]
        [Newtonsoft.Json.JsonProperty("delete", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [XmlElement("delete")]
        public string Delete { get; set; }
    }
}
