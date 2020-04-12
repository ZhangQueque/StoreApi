using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Pages;
using Store.Data;
using Store.Service;

namespace Store.Api.Controllers
{
  
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly StoreDbContext context;
        private readonly IRepositoryWrapper repositoryWrapper;

        public TestController(StoreDbContext context, IRepositoryWrapper repositoryWrapper)
        {
            this.context = context;
            this.repositoryWrapper = repositoryWrapper;
        }

        /// <summary>
        /// 测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        [HttpGet]
        public async Task< IActionResult> Get()
        {
            PageParameters pageParameters = new PageParameters() { };
            var a = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, 5);
             
            return Ok((await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters,5)));
        }
    }
}