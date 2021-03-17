using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodDescriptionDto : TypeDescriptionDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiMethodItem"></param>
        /// <param name="typeDescription"></param>
        public MethodDescriptionDto(IApiMethodItem apiMethodItem, TypeDescription typeDescription)
            : base(typeDescription)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public IApiMethodItem ApiMethodItem { get; set; }
    }
}
