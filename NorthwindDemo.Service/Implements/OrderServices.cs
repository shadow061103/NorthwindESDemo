using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.Entities;
using NorthwindDemo.Service.Interfaces;
using NorthwindDemo.Service.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Service.Implements
{
    /// <summary>
    /// 訂單service
    /// </summary>
    /// <seealso cref="NorthwindDemo.Service.Interfaces.IOrderServices"/>
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uow;

        private IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderServices"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        /// <param name="mapper">The mapper.</param>
        public OrderServices(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// 取得訂單列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OrdersDto>> Get()
        {
            var orders = this._uow.GetRepository<Orders>().Get(
                include:
                c =>
            c.Include(x => x.Customer)
            .Include(x => x.Employee)
            .Include(x => x.ShipViaNavigation)
            .Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Supplier)
            .Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Category))
                .AsEnumerable();

            //var orderDetails = this._uow.GetRepository<OrderDetails>().Get(
            //    include:
            //    c => c.Include(x => x.Product).ThenInclude(x => x.Supplier)
            //    .Include(x => x.Product).ThenInclude(x => x.Category)).AsEnumerable();

            //return orders;
            var orderDtos = this._mapper.Map<IEnumerable<OrdersDto>>(orders);

            return orderDtos;
        }
    }
}