using System;
using System.Collections.Generic;
using System.Linq;

namespace Educ8IT.AspNetCore.SimpleApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResultOfT<T>: ResultOfListOfT<T>
    {
        private long? _PageSize = default;



        /// <summary>
        /// 
        /// </summary>
        public long? PageNumber { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public long? PageSize
        {
            get
            {
                return _PageSize ?? 10;
            }
            set
            {
                if (value.HasValue && value.Value < 1)
                    _PageSize = 10;
                else if (value.HasValue && value.Value > 100)
                    _PageSize = 100;
                else
                    _PageSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPageSize32()
        {
            return Convert.ToInt32(this.PageSize ?? 10);
        }

        /// <summary>
        /// 
        /// </summary>
        public long? PageCount
        {
            get
            {
                if (RecordCount.HasValue)
                    return (RecordCount.Value > 0 ? (RecordCount.Value / PageSize.Value) + 1 : 0);
                else
                    return default;
            }
            set { return; }
        }

        /// <summary>
        /// The count of records available from the operation
        /// </summary>
        public long? RecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OrderByFieldName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ESearchOrderDirection OrderByDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GetRecordsToSkip()
        {
            return (PageSize.HasValue && PageNumber.HasValue)
                    ? (PageSize.Value * (PageNumber.Value - 1))
                    : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetRecordsToSkip32()
        {
            return Convert.ToInt32(this.GetRecordsToSkip());
        }

        /// <summary>
        /// Update counts using the data
        /// </summary>
        public void UpdateFromData()
        {
            if (Data == null)
                return;

            RecordCount = Data.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryResultOfT<T> FromQueryDescription(Dictionary<string, string> query)
        {
            var __resultFilter = new QueryResultOfT<T>()
            {
                RecordCount = -1
            };

            foreach (var item in query.ToList())
            {
                if (String.IsNullOrEmpty(item.Value) || item.Value == "null")
                {
                    query.Remove(item.Key);
                }
            }

            if (query.ContainsKey("pageNumber"))
            {
                if (Int64.TryParse(query["pageNumber"], out long tmp))
                {
                    __resultFilter.PageNumber = tmp;
                }
            }
            if (query.ContainsKey("pageSize"))
            {
                if (Int64.TryParse(query["pageSize"], out long tmp))
                {
                    __resultFilter.PageSize = tmp;
                }
            }
            if (query.ContainsKey("orderBy") && !String.IsNullOrEmpty(query["orderBy"]))
            {
                __resultFilter.OrderByFieldName = query["orderBy"];
            }
            if (query.ContainsKey("orderDirection"))
            {
                if (Int64.TryParse(query["orderDirection"], out long tmp))
                {
                    __resultFilter.OrderByDirection = (ESearchOrderDirection)tmp;
                }
            }

            return __resultFilter;
        }
    }
}
