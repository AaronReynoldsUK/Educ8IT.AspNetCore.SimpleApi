using System;
using System.Linq;
using System.Linq.Expressions;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string fieldName)
        {
            var orderByMethod = "OrderBy";

            return queryable._OrderBy(fieldName, orderByMethod);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, string fieldName)
        {
            var orderByMethod = "OrderByDescending";

            return queryable._OrderBy(fieldName, orderByMethod);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> queryable, string fieldName)
        {
            var orderByMethod = "ThenBy";

            return queryable._OrderBy(fieldName, orderByMethod);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IQueryable<T> queryable, string fieldName)
        {
            var orderByMethod = "ThenByDescending";

            return queryable._OrderBy(fieldName, orderByMethod);
        }


        static IOrderedQueryable<T> _OrderBy<T>(this IQueryable<T> queryable, string fieldName, string orderByMethod)
        {
            if (String.IsNullOrEmpty(fieldName))
                return (IOrderedQueryable<T>)queryable;

            var entityType = typeof(T);
            var propertyInfo = entityType.GetProperty(fieldName);

            ParameterExpression pe = Expression.Parameter(queryable.ElementType);
            MemberExpression me = Expression.Property(pe, fieldName);

            MethodCallExpression orderByCall = Expression.Call(typeof(Queryable),
                orderByMethod,
                new Type[] { queryable.ElementType, me.Type },
                queryable.Expression,
                Expression.Quote(Expression.Lambda(me, pe)));

            return queryable.Provider.CreateQuery(orderByCall) as IOrderedQueryable<T>;
        }

    }
}
