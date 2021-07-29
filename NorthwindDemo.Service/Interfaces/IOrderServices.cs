using NorthwindDemo.Service.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDemo.Service.Interfaces
{
    public interface IOrderServices
    {
        /// <summary>
        /// 取得訂單列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OrdersDto>> Get();
    }
}