using NorthwindDemo.Service.Models;
using NorthwindDemo.Service.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDemo.Service.Interfaces
{
    public interface IOrderESService
    {
        /// <summary>
        /// 新增一筆訂單到ES
        /// </summary>
        /// <param name="ordersDto">The orders dto.</param>
        /// <returns></returns>
        Task<bool> Add(IEnumerable<OrdersDto> ordersDto);

        /// <summary>
        /// 更新訂單到ES
        /// </summary>
        /// <param name="ordersDto">The orders dto.</param>
        /// <returns></returns>
        Task<bool> Update(IEnumerable<OrdersDto> ordersDto);

        /// <summary>
        /// 刪除訂單ES
        /// </summary>
        /// <param name="orderIds">The order ids.</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<int> orderIds);

        /// <summary>
        /// 依編號取得訂單
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        Task<OrdersDto> Get(int orderId);

        /// <summary>
        /// 依條件查詢訂單
        /// </summary>
        /// <param name="searchOrderDto">The search order dto.</param>
        /// <returns></returns>
        Task<IEnumerable<OrdersDto>> Get(SearchOrderDto searchOrderDto);
    }
}