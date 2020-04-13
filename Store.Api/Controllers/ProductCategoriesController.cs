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

        public ProductCategoriesController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// 获取商品类别树结构列表
        /// </summary>
        /// <param name="id">商品类别id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product_CategoryDto>>> GetTree(int id)
        {
            byte[] bytes =await distributedCache.GetAsync($"Product_Category_{id}");
            IEnumerable<Product_CategoryDto> data = Enumerable.Empty<Product_CategoryDto>();
            if (bytes == null)
            {
                data = await repositoryWrapper.Product_CategoryRepository.GetTreeProduct_CategoryDtoes(id);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                     AbsoluteExpiration = DateTime.Now.AddDays(1)
                };
                using (MemoryStream stream = new MemoryStream())
                {
                      Serializer.Serialize(stream, data);
                      await distributedCache.SetAsync($"Product_Category_{id}", stream.ToArray(), options);
                }
                return data.ToList();
            }
            using (MemoryStream stream = new MemoryStream(bytes))
            {

                 data= Serializer.Deserialize<IEnumerable<Product_CategoryDto>>(stream);
                
            }
            return data.ToList();
        }
    }
}