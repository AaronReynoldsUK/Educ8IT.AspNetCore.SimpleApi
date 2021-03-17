using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerDescriptionDto : TypeDescriptionDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiControllerItem"></param>
        /// <param name="typeDescription"></param>
        public ControllerDescriptionDto(IApiControllerItem apiControllerItem, TypeDescription typeDescription)
            : base(typeDescription)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public IApiControllerItem ApiControllerItem { get; set; }
    }
}
