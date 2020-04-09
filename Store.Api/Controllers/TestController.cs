using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data;

namespace Store.Api.Controllers
{
  
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly StoreDbContext context;

        
        public TestController(StoreDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.product_Categories.ToList());
        }
    }
}