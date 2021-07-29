using NorthwindDemo.Repository.Models.ES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDemo.Repository.Interfaces
{
    public interface IOrderESRepository
    {
        /// <summary>
        /// 批次新增
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        Task<bool> BulkInsert(IEnumerable<OrdersESModel> orders);

        /// <summary>
        /// 批次更新
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns></returns>
        Task<bool> BulkUpdate(IEnumerable<OrdersESModel> orders);

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="orderIds">The order ids.</param>
        /// <returns></returns>
        Task<bool> BulkDelete(IEnumerable<int> orderIds);

        /// <summary>
        /// 依編號取得訂單
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        Task<OrdersESModel> GetAsync(int orderId);
    }
}