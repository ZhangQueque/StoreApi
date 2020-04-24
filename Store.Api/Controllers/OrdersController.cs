using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrdersController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
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
        public async Task<IActionResult> AddOrderAsync(Order order)
        {
             
            order.CreateTime = DateTime.Now;
            order.UserId= Convert.ToInt32(User.Identity.Name);
            var buyProduct = await _repositoryWrapper.ProductRepository.GetByIdAsync(order.ProductId);
            if (buyProduct==null)
            {
                return NotFound();
            }
            buyProduct.Stock = buyProduct.Stock - order.Count;

            await _repositoryWrapper.ProductRepository.UpdateAsync(buyProduct);
            if (!  await _repositoryWrapper.ProductRepository.SaveAsync())
            {
                return BadRequest();
            }

            await _repositoryWrapper.OrderRepository.AddAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetCartsAsync),null, order);
        }

        /// <summary>
        /// 订单删除
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            await _repositoryWrapper.OrderRepository.DeleteAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}