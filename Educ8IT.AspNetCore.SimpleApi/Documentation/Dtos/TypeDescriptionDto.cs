using Educ8IT.AspNetCore.SimpleApi.TypeDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Documentation.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class TypeDescriptionDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeDescription"></param>
        public TypeDescriptionDto(TypeDescription typeDescription)
        {
            //this.TypeName = typeDescription.TypeName;
            this.Name = typeDescription.Name;
            this.Description = typeDescription.Description;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
