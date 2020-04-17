using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Store.Dto;
using Store.Service;
using ProtoBuf;
using System.IO;
using Store.Api.RedisCache;
using Store.Api.Attributes;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 商品类别API
    /// </summary>
    [Route("api/cates")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IDistributedCache distributedCache;
        private readonly RedisCacheHelper cacheHelper;

        public ProductCategoriesController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache,RedisCacheHelper cacheHelper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.distributedCache = distributedCache;
            this.cacheHelper = cacheHelper;
        }

        /// <summary>
        /// 获取商品类别树结构列表
        /// </summary>
        /// <param name="id">商品类别id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        
        public async Task<ActionResult<IEnumerable<Product_CategoryDto>>> GetTreeAsync(int id)
        {
            byte[] bytes =await distributedCache.GetAsync($"Product_Category_{id}");
            IEnumerable<Product_CategoryDto> data = Enumerable.Empty<Product_CategoryDto>();
            if (bytes == null)
            {
                data = await repositoryWrapper.Product_CategoryRepository.GetTreeProduct_CategoryDtoesAsync(id);
                await cacheHelper.SetRedisCacheAsync($"Product_Category_{id}",data);
                //DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                //{
                //    SlidingExpiration = TimeSpan.FromHours(3)
                //};
                //using (MemoryStream stream = new MemoryStream())
                //{
                //      Serializer.Serialize(stream, data);
                //      await distributedCache.SetAsync($"Product_Category_{id}", stream.ToArray(), options);
                //}
                return data.ToList();
            }
            //using (MemoryStream stream = new MemoryStream(bytes))
            //{

            //     data= Serializer.Deserialize<IEnumerable<Product_CategoryDto>>(stream);

            //}
            data = await cacheHelper.GetRedisCacheAsync<IEnumerable<Product_CategoryDto>>(bytes);
            return data.ToList();
        }
    }
}