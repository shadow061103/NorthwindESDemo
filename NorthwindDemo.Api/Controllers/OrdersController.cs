using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Api.Models.Parameter;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Service.Interfaces;
using NorthwindDemo.Service.Models;
using System.Threading.Tasks;

namespace NorthwindDemo.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderESService _orderESService;

        private readonly IMapper _mapper;

        public OrdersController(IOrderESService orderESService, IMapper mapper)
        {
            _orderESService = orderESService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [CoreProfilingAsync("OrdersController.GetById")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var temp = await _orderESService.Get(orderId);

            return Ok(temp);
        }

        [HttpGet]
        [CoreProfilingAsync("OrdersController.GetByRule")]
        public async Task<IActionResult> GetByRule([FromQuery] SearchOrderParameter parameter)
        {
            var para = this._mapper.Map<SearchOrderDto>(parameter);

            var orders = await _orderESService.Get(para);

            return Ok(orders);
        }
    }
}