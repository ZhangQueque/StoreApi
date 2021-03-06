﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

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
       [Authorize(Roles ="管理员,员工")]
 
        public IActionResult Get()
        {
            //var list =await repositoryWrapper.Product_CategoryRepository.GetAllAsync();
            //var list2 = await repositoryWrapper.Product_CategoryRepository.GetTreeProduct_CategoryDtoes(1);

            //distributedCache.SetString("name","你好啊");
            //var name = distributedCache.GetString("name");
            //PageParameters pageParameters = new PageParameters() { };
            //var a = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, 5);

            // return Ok((await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters,5)));
            var a= User.Claims;
            return Ok(User.Identity.Name);
        }
    }
}