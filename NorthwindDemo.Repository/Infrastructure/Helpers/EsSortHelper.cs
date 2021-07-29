using Nest;
using NorthwindDemo.Common.Enum;
using NorthwindDemo.Repository.Models.ES;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NorthwindDemo.Repository.Infrastructure.Helpers
{
    public class EsSortHelper
    {
        public static List<ISort> GetOrder(OrderField field)
        {
            var sorts = new List<ISort>();
            switch (field)
            {
                case OrderField.Default:
                    sorts.AddRange(GetFieldSort(c => c.OrderId, SortOrder.Ascending));
                    break;

                case OrderField.OrderDate:
                    sorts.AddRange(GetFieldSort(c => c.OrderDate, SortOrder.Descending));
                    break;

                case OrderField.Freight:
                    sorts.AddRange(GetFieldSort(c => c.Freight, SortOrder.Ascending));
                    break;
            }
            return sorts;
        }

        /// <summary>
        /// 取得欄位排序
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private static IList<ISort> GetFieldSort(Expression<Func<OrdersESModel, object>> field, SortOrder order)
        {
            return new List<ISort>
            {
                new FieldSort
                {
                    Field = Infer.Field<OrdersESModel>(field),
                    Order = order
                }
            };
        }
    }
}