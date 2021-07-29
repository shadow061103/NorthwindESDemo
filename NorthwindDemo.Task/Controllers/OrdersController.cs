using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CoreProfilingAsyncAttribute("TestController.Get")]
        public async Task<IActionResult> Get()
        {
            var temp = await _orderServices.Get();
            return Ok(temp.FirstOrDefault());
        }
    }
}