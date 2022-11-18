using System;
using System.Collections.Generic;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultOfListOfT<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return Data != null; }
        }
    }
}
