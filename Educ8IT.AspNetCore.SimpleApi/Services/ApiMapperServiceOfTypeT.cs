using Educ8IT.AspNetCore.SimpleApi.ApiMapping;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiMapperService<T>: ApiMapperService
    {
        #region Private Fields

        private IApiDescription _apiDescription;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="apiMapperOptions"></param>
        public ApiMapperService(
            ILoggerFactory loggerFactory,
            IOptions<ApiMapperOptions> apiMapperOptions)
            : base(loggerFactory, apiMapperOptions)
        { }

        #endregion

        #region Properties

        // TODO: make IApiDescription ApiDescription property abstract on base class

        #endregion

        #region Public Methods


        #endregion
    }
}
