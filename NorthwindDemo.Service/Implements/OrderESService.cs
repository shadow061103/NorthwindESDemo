using AutoMapper;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.ES;
using NorthwindDemo.Service.Interfaces;
using NorthwindDemo.Service.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Service.Implements
{
    public class OrderESService : IOrderESService
    {
        private readonly IOrderESRepository _orderESRepository;

        private readonly IMapper _mapper;

        public OrderESService(IOrderESRepository orderESRepository, IMapper mapper)
        {
            _orderESRepository = orderESRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增一筆訂單到ES
        /// </summary>
        /// <param name="ordersDto">The orders dto.</param>
        /// <returns></returns>
        [CoreProfilingAsync("OrderESService.Get")]
        public async Task<bool> Add(IEnumerable<OrdersDto> ordersDto)
        {
            if (ordersDto.Any().Equals(false))
            {
                return false;
            }

            var orders = this._mapper.Map<IEnumerable<OrdersESModel>>(ordersDto);

            return await _orderESRepository.BulkInsert(orders);
        }

        /// <summary>
        /// 更新訂單到ES
        /// </summary>
        /// <param name="ordersDto">The orders dto.</param>
        /// <returns></returns>
        [CoreProfilingAsync("OrderESService.Update")]
        public async Task<bool> Update(IEnumerable<OrdersDto> ordersDto)
        {
            if (ordersDto.Any().Equals(false))
            {
                return false;
            }

            var orders = this._mapper.Map<IEnumerable<OrdersESModel>>(ordersDto);

            return await _orderESRepository.BulkUpdate(orders);
        }

        /// <summary>
        /// 刪除訂單ES
        /// </summary>
        /// <param name="orderIds">The order ids.</param>
        /// <returns></returns>
        [CoreProfilingAsync("OrderESService.Delete")]
        public async Task<bool> Delete(IEnumerable<int> orderIds)
        {
            if (orderIds.Any().Equals(false))
            {
                return false;
            }

            return await _orderESRepository.BulkDelete(orderIds);
        }
    }
}