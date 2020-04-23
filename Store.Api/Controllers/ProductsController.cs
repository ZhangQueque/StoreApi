﻿using System;
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

        public ProductsController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache, IMapper mapper , RedisCacheHelper cacheHelper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.distributedCache = distributedCache;
            this.mapper = mapper;
            this.cacheHelper = cacheHelper;
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
                if (pageParameters.IsPriceSort!=null)
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
        public async Task<Product> GetProductById(int id)
        {
            Product product;
            byte[] bytes = distributedCache.Get($"Product_{id}");
            if (bytes==null)
            {
                product = await repositoryWrapper.ProductRepository.GetProductById(id);
                await cacheHelper.SetRedisCacheAsync($"Product_{id}", product);
                return product;
            }
            product = await cacheHelper.GetRedisCacheAsync<Product>(bytes);
            return product;
        }


        /// <summary>
        /// 最新商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("new")]
        public async Task<ActionResult<List<Product>>> GetNewProductsAsync()
        {
            var data =Enumerable.Empty<Product>();
            var bytes = await distributedCache.GetAsync("GetNewProductsAsync");
            if (bytes==null)
            {
                data = await repositoryWrapper.ProductRepository.GetNewProductsAsync();
                await cacheHelper.SetRedisCacheAsync<List<Product>>("GetNewProductsAsync", data.ToList());
                return data.ToList();
            }
            data= await cacheHelper.GetRedisCacheAsync<List<Product>>(bytes);
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
    }

   
}