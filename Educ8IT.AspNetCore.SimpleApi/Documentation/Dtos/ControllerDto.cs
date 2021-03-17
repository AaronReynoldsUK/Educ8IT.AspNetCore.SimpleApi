using Educ8IT.AspNetCore.SimpleApi.Attributes;
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
    [DataContract]
    [Serializable]
    public class ControllerDto
    {
        /// <summary>
        /// 
        /// </summary>
        public ControllerDto() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiControllerItem"></param>
        /// <param name="documentationPaths"></param>
        public ControllerDto(IApiControllerItem apiControllerItem, DocumentationPaths documentationPaths)
        {
            ApiControllerItem = apiControllerItem;

            this.Name = apiControllerItem.Name;
            this.Description = apiControllerItem.Description;
            this.RoutePrefixes = apiControllerItem.RoutePrefixes;

            documentationPaths.UpdateController(this.Name);
            documentationPaths.ControllerRoutePaths = this.RoutePrefixes;
            DocumentationPaths = documentationPaths;

            foreach (var __method in apiControllerItem.Methods)
            {
                Methods.Add(new MethodDto(__method, documentationPaths.Clone()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IApiControllerItem ApiControllerItem { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DocumentationPaths DocumentationPaths { get; set; }

        public string Name { get; set; }

        public string TypeName
        {
            get
            {
                return ApiControllerItem.ControllerType.Name;
            }
        }

        public string TypeFullName
        {
            get
            {
                return ApiControllerItem.ControllerType.FullName;
            }
        }

        public string Description { get; set; }


        public string RouteName
        {
            get
            {
                return ApiControllerItem.RouteName;
            }
        }

        public List<MethodDto> Methods { get; set; } = new List<MethodDto>();

        public List<string> RoutePrefixes { get; set; } = new List<string>();

        public List<ApiVersion> Versions
        {
            get
            {
                return ApiControllerItem.Versions;
            }
        }

        // Attributes...

    }
}
