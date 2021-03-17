using Educ8IT.AspNetCore.SimpleApi.Attributes;
using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Newtonsoft.Json;
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
    public class DocumentationPaths
    {
        /// <summary>
        /// 
        /// </summary>
        public DocumentationPaths() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentationPaths"></param>
        public DocumentationPaths(DocumentationPaths documentationPaths)
        {
            RootDocumentationUriTemplate = documentationPaths.RootDocumentationUriTemplate;
            ControllerDocumentationUriTemplate = documentationPaths.ControllerDocumentationUriTemplate;
            MethodDocumentationUriTemplate = documentationPaths.MethodDocumentationUriTemplate;
            TypeDocumentationUriTemplate = documentationPaths.TypeDocumentationUriTemplate;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RootDocumentationUriTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ControllerDocumentationUriTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MethodDocumentationUriTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDocumentationUriTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ControllerRoutePaths { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerName"></param>
        public void UpdateController(string controllerName)
        {
            ControllerDocumentationUriTemplate = ControllerDocumentationUriTemplate
                .Replace("[CONTROLLER]", controllerName);
            MethodDocumentationUriTemplate = MethodDocumentationUriTemplate
                .Replace("[CONTROLLER]", controllerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        public void UpdateMethod(string methodName)
        {
            MethodDocumentationUriTemplate = MethodDocumentationUriTemplate
                .Replace("[METHOD]", methodName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DocumentationPaths Clone()
        {
            return new DocumentationPaths(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    public class VersionedApiDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionedControllers"></param>
        public VersionedApiDto(IDictionary<ApiVersion, List<IApiControllerItem>> versionedControllers, DocumentationPaths documentationPaths)
        {
            if (versionedControllers == null)
                return;

            DocumentationPaths = documentationPaths;

            foreach (var item in versionedControllers)
            {
                var __list = new List<ControllerDto>();
                foreach (var controller in item.Value)
                {
                    __list.Add(new ControllerDto(controller, documentationPaths.Clone()));
                }
                VersionedControllers.Add(item.Key.ToString(), __list);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, List<ControllerDto>> VersionedControllers { get; private set; } = new
            Dictionary<string, List<ControllerDto>>();

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public DocumentationPaths DocumentationPaths { get; set; }
    }

    [DataContract]
    [JsonObject]
    public class ListOfControllerDto: List<ControllerDto>
    {

    }

}
