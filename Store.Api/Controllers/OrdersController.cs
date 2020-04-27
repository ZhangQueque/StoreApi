using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entities;
using Store.Dto;
using Store.Service;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly StoreDbContext _context;

        public OrdersController(IRepositoryWrapper repositoryWrapper, IMapper mapper, StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
            this._context = context;

        }

        /// <summary>
        /// 返回用户对应的订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCartsAsync()
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            var data = await _repositoryWrapper.OrderRepository.GetOrdersAsync(userId);

            return data.ToList();
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="order">新增对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync([FromBody]Order order)
        {

            order.CreateTime = DateTime.Now;
            order.UserId = Convert.ToInt32(User.Identity.Name);
            var buyProduct = await _repositoryWrapper.ProductRepository.GetByIdAsync(order.ProductId);
            if (buyProduct == null)
            {
                return NotFound("该商品不存在！");
            }
            if (buyProduct.Stock == 0)
            {
                return NotFound("该商品已下架！");
            }
            buyProduct.Stock = buyProduct.Stock - order.Count;
            buyProduct.Purchase = buyProduct.Purchase + order.Count;
            await _repositoryWrapper.ProductRepository.UpdateAsync(buyProduct);
            if (!await _repositoryWrapper.ProductRepository.SaveAsync())
            {
                return BadRequest();
            }

            await _repositoryWrapper.OrderRepository.AddAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            if (order.Status == 1)
            {
                var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "Order");
                commonData.Value = commonData.Value + order.Count;
                _context.CommonDatas.Update(commonData);
                await _context.SaveChangesAsync();
                LogMessage logMessage = new LogMessage { Content = $" “{User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value}” 购买了：“{buyProduct.Title}”{order.Count}件！", CreateTime = DateTime.Now };
                await _context.LogMessages.AddAsync(logMessage);
                await _context.SaveChangesAsync();
            }
     

            return Ok();
        }

        /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var buyProduct = await _repositoryWrapper.ProductRepository.GetByIdAsync(order.ProductId);
            if (buyProduct == null)
            {
                return NotFound("该商品不存在！");
            }

            order.Status = 4;
            await _repositoryWrapper.OrderRepository.UpdateAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }

            buyProduct.Stock = buyProduct.Stock + order.Count;
            buyProduct.Purchase = buyProduct.Purchase - order.Count;
            await _repositoryWrapper.ProductRepository.UpdateAsync(buyProduct);
            if (!await _repositoryWrapper.ProductRepository.SaveAsync())
            {
                return BadRequest();
            }


            var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "Order");
            commonData.Value = commonData.Value - order.Count;
            _context.CommonDatas.Update(commonData);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}