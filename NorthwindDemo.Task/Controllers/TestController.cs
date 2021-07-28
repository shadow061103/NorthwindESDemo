using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Repository.Interfaces;
using NorthwindDemo.Repository.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDemo.Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController :  ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public TestController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CoreProfilingAsyncAttribute("TestController.Get")]
        public IActionResult Get()
        {
          var temp= _uow.GetRepository<Employees>().Get();
            return Ok(temp);
        }
    }
}
