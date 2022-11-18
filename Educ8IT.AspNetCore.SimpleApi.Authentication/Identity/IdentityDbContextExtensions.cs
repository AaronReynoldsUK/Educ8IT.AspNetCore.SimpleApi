using Educ8IT.AspNetCore.SimpleApi.Common;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Authentication.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public static class IdentityDbContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="searchQuery"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static async Task<QueryResultOfT<SimpleApi.Identity.ApiMfa>> SearchMfa(
            this SimpleApi.Identity.IdentityDbContext dbContext,
            Dictionary<string, string> searchQuery,
            QueryResultOfT<SimpleApi.Identity.ApiMfa> result)
        {
            var __builder = PredicateBuilder.New<SimpleApi.Identity.ApiMfa>(true);

            if (searchQuery.TryGetValue("friendlyName", out string friendlyName) && !String.IsNullOrEmpty(friendlyName))
                __builder = __builder.And(e => !String.IsNullOrEmpty(e.FriendlyName) && e.FriendlyName.Contains(friendlyName));

            if (!String.IsNullOrEmpty(result.OrderByFieldName))
            {
                await result.PerformOrderedQuery(dbContext.ApiMfas
                    .AsNoTracking()
                    .AsExpandableEFCore(), __builder);
            }
            else
            {
                await result.PerformUnorderedQuery(dbContext.ApiMfas
                    .AsNoTracking()
                    .AsExpandableEFCore(), __builder);
            }

            return result;
        }
    }
}
