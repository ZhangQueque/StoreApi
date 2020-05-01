using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using Store.Core.Pages;
using Store.Data.Entities;
using Store.Dto;
using Store.Service;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Store.Api.RedisCache;
using Store.Api.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Store.Data;
 
using Microsoft.EntityFrameworkCore;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 商品
    /// </summary>
    [Route("api/{typeId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IDistributedCache distributedCache;
        private readonly IMapper mapper;
        private readonly RedisCacheHelper cacheHelper;
        private readonly StoreDbContext _context;

        public ProductsController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache, IMapper mapper, RedisCacheHelper cacheHelper, StoreDbContext context)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.distributedCache = distributedCache;
            this.mapper = mapper;
            this.cacheHelper = cacheHelper;
            this._context = context;
        }

        /// <summary>
        /// 商品信息列表
        /// </summary>
        /// <param name="typeId">商品类别</param>
        /// <param name="pageParameters">查询参数</param>
        /// <returns></returns>     
        [HttpGet]
        [ServiceFilter(typeof(IsExistProductAttribute))]

        public async Task<ActionResult<PageList<Product>>> GetProductsAsync(int typeId, [FromQuery]PageParameters pageParameters)
        {

            PageList<Product> data = new PageList<Product>();

            //名称查询缓冲
            if (!string.IsNullOrEmpty(pageParameters.Name))
            {
                //价格排序缓冲
                if (pageParameters.IsPriceSort != null)
                {
                    byte[] pricebytes = await distributedCache.GetAsync($"Product_Price_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsPriceSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                    if (pricebytes == null)
                    {
                        data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                        await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Price_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsPriceSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                        return data;
                    }
                    else
                    {
                        data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(pricebytes);
                        return data;
                    }
                }

                //购买量查询
                if (pageParameters.IsPurchaseSort != null)
                {
                    byte[] purchasebytes = await distributedCache.GetAsync($"Product_Purchase_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsPurchaseSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                    if (purchasebytes == null)
                    {
                        data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                        await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Purchase_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsPurchaseSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                        return data;
                    }
                    else
                    {
                        data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(purchasebytes);
                        return data;
                    }
                }

                //时间查询
                if (pageParameters.IsTimeSort != null)
                {
                    byte[] timebytes = await distributedCache.GetAsync($"Product_Time_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsTimeSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                    if (timebytes == null)
                    {
                        data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                        await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Time_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_{pageParameters.IsTimeSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                        return data;
                    }
                    else
                    {
                        data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(timebytes);
                        return data;
                    }
                }

                //名称查询
                byte[] bytes = await distributedCache.GetAsync($"Product_Name_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                if (bytes == null)
                {
                    data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                    await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Name_{typeId}_{pageParameters.PageIndex}_{pageParameters.Name}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                    return data;
                }
                else
                {
                    data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(bytes);
                    return data;
                }
            }

            //价格排序缓冲
            if (pageParameters.IsPriceSort != null)
            {
                byte[] pricebytes = await distributedCache.GetAsync($"Product_Price_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsPriceSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                if (pricebytes == null)
                {
                    data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                    await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Price_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsPriceSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                    return data;
                }
                else
                {
                    data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(pricebytes);
                    return data;
                }
            }

            //购买量查询
            if (pageParameters.IsPurchaseSort != null)
            {
                byte[] purchasebytes = await distributedCache.GetAsync($"Product_Purchase_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsPurchaseSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                if (purchasebytes == null)
                {
                    data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                    await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Purchase_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsPurchaseSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                    return data;
                }
                else
                {
                    data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(purchasebytes);
                    return data;
                }
            }

            //时间查询
            if (pageParameters.IsTimeSort != null)
            {
                byte[] timebytes = await distributedCache.GetAsync($"Product_Time_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsTimeSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
                if (timebytes == null)
                {
                    data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                    await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_Time_{typeId}_{pageParameters.PageIndex}_{pageParameters.IsTimeSort}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                    return data;
                }
                else
                {
                    data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(timebytes);
                    return data;
                }
            }

            //正常查询
            byte[] noParameterbytes = await distributedCache.GetAsync($"Product_{typeId}_{pageParameters.PageIndex}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}");
            if (noParameterbytes == null)
            {
                data = await repositoryWrapper.ProductRepository.GetPageListsAsync(pageParameters, typeId);
                await cacheHelper.SetRedisCacheAsync<PageList<Product>>($"Product_{typeId}_{pageParameters.PageIndex}_Price_{pageParameters.BottomPrice}_{pageParameters.TopPrice}", data);
                return data;
            }
            else
            {
                data = await cacheHelper.GetRedisCacheAsync<PageList<Product>>(noParameterbytes);
                return data;
            }
        }


        /// <summary>
        /// 获取单个商品信息（及时加载）
        /// </summary>
        /// <param name="id">商品主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product;

            product = await repositoryWrapper.ProductRepository.GetProductById(id);
            if (product == null)
            {
                product = new Product();
                return product;
            }

            product.PageView = product.PageView + 1;
            await repositoryWrapper.ProductRepository.UpdateAsync(product);
            await repositoryWrapper.ProductRepository.SaveAsync();
            return product;
        }


        /// <summary>
        /// 获取商品下的所有图片
        /// </summary>
        /// <param name="id">商品主键</param>
        /// <returns></returns>
        [HttpGet("images/{id}")]
        public async Task<List<Product_Image>> GetProductImagesAsync(int id)
        {

            var data = _context.Product_Images.Where(m => m.ProductId == id);


            return await data.ToListAsync();
        }


        /// <summary>
        /// 商品图片添加
        /// </summary>
        /// <param name="id">商品主键</param>
        /// <param name="product_ImageDto">图片对象</param>
        /// <returns></returns>
        [HttpPost("images/{id}")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]

        public async Task<IActionResult> CreateProductImagesAsync(int id,[FromForm]Product_ImageDto product_ImageDto)
        {
            if (product_ImageDto.Image1!=null && product_ImageDto.Image2!=null)
            {
                var images = await _context.Product_Images.Where(m => m.ProductId == id).ToListAsync();
                _context.Product_Images.RemoveRange(images.ToArray());
                await _context.SaveChangesAsync();
            }
            var count = await _context.Product_Images.Where(m=>m.ProductId==id).CountAsync();
            if (count>1)
            {
                var images = await _context.Product_Images.Where(m => m.ProductId == id).OrderBy(m=>m.Id).ToListAsync();
                 _context.Product_Images.RemoveRange(images.ToArray().FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            Product_Image product_Image = new Product_Image();
            if (product_ImageDto.Image1 != null)
            {
                var extensionPath = Path.GetExtension(product_ImageDto.Image1.FileName);
                string fileName = id + "_" + id + "_" + product_ImageDto.Image1.FileName.Substring(0, 4) + extensionPath;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImg", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await product_ImageDto.Image1.CopyToAsync(stream);
                }
                string imgurl = Request.Scheme + "://" + Request.Host;
                product_Image.Image = imgurl + "/ProductImg/" + fileName;
                product_Image.ProductId = id;
                await _context.Product_Images.AddAsync(product_Image);
                await _context.SaveChangesAsync();
            }



            Product_Image product_Image2 = new Product_Image();
            if (product_ImageDto.Image2 != null)
            {
                var extensionPath2 = Path.GetExtension(product_ImageDto.Image2.FileName);
                string fileName2 = id + "_" + id + "_" + product_ImageDto.Image2.FileName.Substring(0, 4) + extensionPath2;
                string path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImg", fileName2);
                using (var stream = System.IO.File.Create(path2))
                {
                    await product_ImageDto.Image2.CopyToAsync(stream);
                }
                string imgur2 = Request.Scheme + "://" + Request.Host;
                product_Image2.Image = imgur2 + "/ProductImg/" + fileName2;
                product_Image2.ProductId = id;

                await _context.Product_Images.AddAsync(product_Image2);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        /// <summary>
        /// 最新商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("new")]
        public async Task<ActionResult<List<Product>>> GetNewProductsAsync()
        {

            var data = await repositoryWrapper.ProductRepository.GetNewProductsAsync();

            return data.ToList();
        }

        /// <summary>
        /// 销量最高商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("shoptop")]
        public async Task<ActionResult<List<Product>>> GetShopTopProductsAsync()
        {
            var data = Enumerable.Empty<Product>();
            var bytes = await distributedCache.GetAsync("GetShopTopProductsAsync");
            if (bytes == null)
            {
                data = await repositoryWrapper.ProductRepository.GetShopTopProductsAsync();
                await cacheHelper.SetRedisCacheAsync<List<Product>>("GetShopTopProductsAsync", data.ToList());
                return data.ToList();
            }
            data = await cacheHelper.GetRedisCacheAsync<List<Product>>(bytes);
            return data.ToList();
        }

        /// <summary>
        /// 推荐商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("viewtop")]
        public async Task<ActionResult<List<Product>>> GetPageViewTopProductsAsync()
        {
            var data = Enumerable.Empty<Product>();
            var bytes = await distributedCache.GetAsync("GetPageViewTopProductsAsync");
            if (bytes == null)
            {
                data = await repositoryWrapper.ProductRepository.GetPageViewTopProductsAsync();
                await cacheHelper.SetRedisCacheAsync<List<Product>>("GetPageViewTopProductsAsync", data.ToList());
                return data.ToList();
            }
            data = await cacheHelper.GetRedisCacheAsync<List<Product>>(bytes);
            return data.ToList();
        }


        /// <summary>
        /// 商品创建
        /// </summary>
        /// <param name="productCreateDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]

        public async Task<IActionResult> CreateProductAsync([FromForm]ProductCreateDto productCreateDto)
        {
            Product product = new Product();
            product.Title = productCreateDto.Title;
            product.Price = productCreateDto.Price;

            product.Product_CategoryId = productCreateDto.Product_CategoryId;

            product.ShortDescribe = productCreateDto.ShortDescribe;
            product.Stock = productCreateDto.Stock;
            product.ProductNo = Guid.NewGuid();
            product.CreateTime = DateTime.Now;
            int userId = Convert.ToInt32(User.Identity.Name);

            product.UserId = userId;
            product.UserName = User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value;
            if (productCreateDto.Pictrue != null)
            {
                var extensionPath = Path.GetExtension(productCreateDto.Pictrue.FileName);
                string fileName = product.Title + "_" + Guid.NewGuid().ToString() + extensionPath;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImg", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await productCreateDto.Pictrue.CopyToAsync(stream);
                }
                string imgurl = Request.Scheme + "://" + Request.Host;
                product.Pictrue = imgurl + "/ProductImg/" + fileName;
            }

            await repositoryWrapper.ProductRepository.AddAsync(product);

            if (!await repositoryWrapper.ProductRepository.SaveAsync())
            {
                return BadRequest();
            }


            return Ok(product.Id);
        }

        /// <summary>
        /// 获取全部商品
        /// </summary>
        /// <param name="index">页索引</param>
        /// <param name="size">每页显示个数</param>
        /// <param name="name">查询名称</param>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]
        public async Task<PageList<Product>> GetProductAllByAdmin(int index = 1, int size = 10, string name = "")
        {
            var data = await repositoryWrapper.ProductRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            {
                data= data.Where(m=>m.Title.Contains(name));
            }
            var source =await PageList<Product>.CreatePageList(data,index,size);
            return source;
        }


        /// <summary>
        /// 商品修改
        /// </summary>
        /// <param name="id">商品主键</param>
        /// <param name="productCreateDto">修改对象</param>
        /// <returns></returns>
        [HttpPost("edit/{id}")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]

        public async Task<IActionResult> EditProductAsync(int id, [FromForm]ProductCreateDto productCreateDto)
        {
            Product product = await repositoryWrapper.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            product.Title = productCreateDto.Title;
            product.Price = productCreateDto.Price;

            if (productCreateDto.Product_CategoryId != 0)
            {
                product.Product_CategoryId = productCreateDto.Product_CategoryId;

            }

            product.ShortDescribe = productCreateDto.ShortDescribe;
            product.Stock = productCreateDto.Stock;
            int userId = Convert.ToInt32(User.Identity.Name);

            product.UserId = userId;
            product.UserName = User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value;
            if (productCreateDto.Pictrue != null)
            {
                var extensionPath = Path.GetExtension(productCreateDto.Pictrue.FileName);
                string fileName = product.Title + "_" + userId + "_" + product.Title.Substring(0, 4) + extensionPath;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImg", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await productCreateDto.Pictrue.CopyToAsync(stream);
                }
                string imgurl = Request.Scheme + "://" + Request.Host;
                product.Pictrue = imgurl + "/ProductImg/" + fileName;
            }

            await repositoryWrapper.ProductRepository.UpdateAsync(product);

            if (!await repositoryWrapper.ProductRepository.SaveAsync())
            {
                return BadRequest();
            }


            return Ok();
        }


        /// <summary>
        /// 添加商品博客
        /// </summary>k
        /// <param name="id">商品主键</param>
        /// <param name="product_Describe">博客对象</param>
        /// <returns></returns>
        [HttpPost("blog/{id}")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]

        public async Task<IActionResult> CreateProductBlog(int id,[FromBody] Product_Describe product_Describe)
        {
            var productblog = await _context.Product_Describes.FirstOrDefaultAsync(m=>m.ProductId==id);
            if (productblog!=null)
            {
                 _context.Product_Describes.Remove(productblog);

                await _context.SaveChangesAsync();
            }
            product_Describe.ProductId = id;
            await _context.Product_Describes.AddAsync(product_Describe);
            await _context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// 更改商品状态
        /// </summary>
        /// <param name="id">商品主键</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<IActionResult> ProductToStatus1(int id ,string key)
        {
            if (key != "zhanghaodong138") //这就不写那些复杂的了，（配置文件，在加密解密，偷懒了）
            {
                return Ok(new { code = 1, msg = "密钥不正确！" });
            }
            var product = await repositoryWrapper.ProductRepository.GetByIdAsync(id);
            product.Status = product.Status == 0 ? 1 : 0;

            await repositoryWrapper.ProductRepository.UpdateAsync(product);
            await repositoryWrapper.ProductRepository.SaveAsync();
            return Ok();
        }
    }


}