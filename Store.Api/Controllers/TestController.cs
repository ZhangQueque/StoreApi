using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Api.Attributes;
using Store.Core.Pages;
using Store.Data;
using Store.Service;
using Microsoft.Extensions.Caching.Distributed;
namespace Store.Api.Controllers
{

    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/test/{id}")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly StoreDbContext context;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly ILogger<TestController> logger;
        private readonly IDistributedCache distributedCache;

        public TestController(StoreDbContext context, IRepositoryWrapper repositoryWrapper, ILogger<TestController> logger, IDistributedCache distributedCache)
        {
            this.context = context;
            this.repositoryWrapper = repositoryWrapper;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// 测试数据
        /// </summary>
        /// <returns>测试数据</returns>
        [HttpGet]//[HttpGet("{id}")]
                 // [ServiceFilter(typeof(IsExistProductAttribute))]
        [ServiceFilter(typeof(IsExistProductAttribute))]
        public async Task<IActionResult> Get(int id)
        {
            //var list =await repositoryWrapper.Product_CategoryRepository.GetAllAsync();
            //var list2 = await repositoryWrapper.Product_CategoryRepository.GetTreeProduct_CategoryDtoes(1);

            //distributedCache.SetString("name","你好啊");
            //var name = distributedCache.GetString("name");
            //PageParameters pageParameters = new PageParameters() { };
            //var a = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, 5);

            // return Ok((await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters,5)));
           
            return Ok();
        }
    }
}