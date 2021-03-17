using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class MethodParameterDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiParameterItem"></param>
        /// <param name="documentationPaths"></param>
        public MethodParameterDto(ApiParameterItem apiParameterItem, DocumentationPaths documentationPaths)
        {
            ApiParameterItem = apiParameterItem;

            this.Name = apiParameterItem.Name;
            this.Description = apiParameterItem.Description;
            this.TypeName = apiParameterItem.TypeName;

            DocumentationPaths = documentationPaths;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ApiParameterItem ApiParameterItem { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DocumentationPaths DocumentationPaths { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TypeName { get; set; }
    }
}
