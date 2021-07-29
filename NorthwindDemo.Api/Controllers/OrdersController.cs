using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        private readonly IOrderESService _orderESService;

        public OrdersController(IOrderServices orderServices, IOrderESService orderESService)
        {
            _orderServices = orderServices;
            _orderESService = orderESService;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CoreProfilingAsync("OrdersController.Get")]
        public async Task<IActionResult> Get()
        {
            var temp = await _orderServices.Get();
            return Ok(temp.FirstOrDefault());
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
    }
}