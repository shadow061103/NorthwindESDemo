using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Service.Interfaces;

namespace NorthwindDemo.Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public TestController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CoreProfilingAsyncAttribute("TestController.Get")]
        public IActionResult Get()
        {
            var temp = _orderServices.Get();
            return Ok(temp);
        }
    }
}