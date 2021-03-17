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
    public class MethodDto
    {
        /// <summary>
        /// 
        /// </summary>
        public MethodDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMethodItem"></param>
        /// <param name="documentationPaths"></param>
        public MethodDto(IApiMethodItem apiMethodItem, DocumentationPaths documentationPaths)
        {
            ApiMethodItem = apiMethodItem;

            this.Name = apiMethodItem.Name;
            this.Description = apiMethodItem.Description;
            this.ActionRoutes = apiMethodItem.ActionRoutes;

            documentationPaths.UpdateMethod(this.Name);
            DocumentationPaths = documentationPaths;

            foreach (var __param in apiMethodItem.MethodParameters)
            {
                Parameters.Add(new MethodParameterDto(__param, documentationPaths.Clone()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IApiMethodItem ApiMethodItem { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DocumentationPaths DocumentationPaths { get; set; }

        public string ControllerName
        {
            get
            {
                return ApiMethodItem.ParentTypeName;
            }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DocumentationUri
        {
            get
            {
                return DocumentationPaths.MethodDocumentationUriTemplate;
            }
        }

        public List<ActionRoute> ActionRoutes { get; set; }

        public List<MethodParameterDto> Parameters { get; set; } = new List<MethodParameterDto>();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public List<ApiParameterItem> MethodParameters
        {
            get
            {
                return ApiMethodItem.MethodParameters;
            }
        }
    }
}
