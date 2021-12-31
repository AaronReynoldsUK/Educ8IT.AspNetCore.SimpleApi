// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.ActionResults
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [XmlRoot("pagedResult")]
    public class PagedResultObject<T>
    {
        /// <summary>
        /// Response Object
        /// </summary>
        //[XmlElement("resultObject")]
        [XmlArray("resultObject")]
        [System.Text.Json.Serialization.JsonPropertyName("resultObject")]
        [Newtonsoft.Json.JsonProperty("resultObject")]
        public T ResultObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("pageNumber")]
        [System.Text.Json.Serialization.JsonPropertyName("pageNumber")]
        [Newtonsoft.Json.JsonProperty("pageNumber")]
        public long? PageNumber { get; set; }

        private long? _PageSize = default;

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("pageSize")]
        [System.Text.Json.Serialization.JsonPropertyName("pageSize")]
        [Newtonsoft.Json.JsonProperty("pageSize")]
        public long? PageSize
        {
            get
            {
                return _PageSize ?? 10;
            }
            set
            {
                if (value.HasValue && value.Value < 1)
                    _PageSize = 10;
                else if (value.HasValue && value.Value > 100)
                    _PageSize = 100;
                else
                    _PageSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("pageCount")]
        [System.Text.Json.Serialization.JsonPropertyName("pageCount")]
        [Newtonsoft.Json.JsonProperty("pageCount")]
        public long? PageCount
        {
            get
            {
                // AR: 2021-12-31: remove DIV by ZERO possibility and return correct values
                if (!RecordCount.HasValue || !PageSize.HasValue)
                    return default;
                else if (PageSize == 0)
                    return default;
                else if (RecordCount == 0)
                    return 0;
                else
                {
                    // AR: 2021-12-31: Fix +1 page count when RecordCount is a multiple of PageSize
                    return (RecordCount / PageSize) + (RecordCount % PageSize == 0 ? 0 : 1);
                }
            }
            set { return; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("recordCount")]
        [System.Text.Json.Serialization.JsonPropertyName("recordCount")]
        [Newtonsoft.Json.JsonProperty("recordCount")]
        public long? RecordCount { get; set; }
    }
}
