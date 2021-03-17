// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// Generic HATEOS result / response object.
    /// </summary>
    [XmlRoot("hateosResult")]
    public class HateosResultObject<T>
    {
        /// <summary>
        /// HATEOS Links for this object
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("links")]
        [Newtonsoft.Json.JsonProperty("links")]
        [XmlElement("links")]
        public HateosResultObjectLinks Links { get; set; }

        /// <summary>
        /// Response Object
        /// </summary>
        //[XmlElement("resultObject")]
        [XmlArray("resultObject")]
        [System.Text.Json.Serialization.JsonPropertyName("resultObject")]
        [Newtonsoft.Json.JsonProperty("resultObject")]
        public List<T> ResultObject { get; set; }
    }
}
