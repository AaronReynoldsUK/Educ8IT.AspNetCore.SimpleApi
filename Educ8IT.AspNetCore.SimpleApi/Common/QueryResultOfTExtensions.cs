using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryResultOfTExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="queryable"></param>
        /// <param name="expression"></param>
        public static void UpdateRecordCount<T>(this QueryResultOfT<T> result, IQueryable<T> queryable, ExpressionStarter<T> expression)
        {
            result.RecordCount = queryable.Where(expression).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="queryable"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task PerformUnorderedQuery<T>(this QueryResultOfT<T> result, IQueryable<T> queryable, ExpressionStarter<T> expression)
        {
            if (result.RecordCount <= 0)
            {
                result.UpdateRecordCount(queryable, expression);
            }

            result.Data = await
                (result.PageSize.HasValue
                   ? queryable.Where(expression).Skip(result.GetRecordsToSkip32()).Take(result.GetPageSize32())
                   : queryable.Where(expression)
                ).ToListAsync<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="queryable"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task PerformOrderedQuery<T>(this QueryResultOfT<T> result, IQueryable<T> queryable, ExpressionStarter<T> expression)
        {
            if (result.RecordCount <= 0)
            {
                result.UpdateRecordCount(queryable, expression);
            }

            var __orderedQueryable = (result.OrderByDirection == ESearchOrderDirection.Ascending)
                ? queryable.OrderBy(result.OrderByFieldName)
                : queryable.OrderByDescending(result.OrderByFieldName);

            result.Data = await
                (result.PageSize.HasValue
                   ? __orderedQueryable.Where(expression).Skip(result.GetRecordsToSkip32()).Take(result.GetPageSize32())
                   : __orderedQueryable.Where(expression)
                ).ToListAsync<T>();
        }
    }
}
