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
        /// 获取全部订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCartsAllAsync()
        {
           
            var data = await _repositoryWrapper.OrderRepository.GetOrdersAllAsync();

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
        /// 订单支付 （1代表订单已支付）
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("status1/{id}")]
        public async Task<IActionResult> UpdateStatusTo1(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != 0)  //没有订单记录不能付款
            {
                return BadRequest();
            }
            order.Status = 1;
            await _repositoryWrapper.OrderRepository.UpdateAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// 订单发货（2代表订单已发货）
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("status2/{id}")]
        [Authorize(Roles = "帮主,副帮主,帮主夫人")]
        public async Task<IActionResult> UpdateStatusTo2(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != 1) //没有付款不能发货
            {
                return BadRequest();
            }
            order.Status = 2;

            await _repositoryWrapper.OrderRepository.UpdateAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// 确认收货（3代表订单已收货）
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("status3/{id}")]
        public async Task<IActionResult> UpdateStatusTo3(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != 2 && order.Status != 5)  //没有发货不能收货 //退货状态取消
            {
                return BadRequest();
            }
            order.Status = 3;
            await _repositoryWrapper.OrderRepository.UpdateAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// 申请退款（5代表申请退款）
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("status5/{id}")]
        public async Task<IActionResult> UpdateStatusTo5(int id)
        {
            var order = await _repositoryWrapper.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != 3) //不收货不能退款
            {
                return BadRequest();
            }
            order.Status = 5;

            await _repositoryWrapper.OrderRepository.UpdateAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok("申请退款成功！");
        }

        /// <summary>
        /// 订单取消 (4代表订单已取消，交易关闭)
        /// </summary>
        /// <param name="id">订单主键</param>
        /// <returns></returns>
        [HttpGet("status4/{id}")]
        public async Task<IActionResult> UpdateStatusTo4(int id)
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
            if (order.Status == 2)  //发货了就不能取消了，只能收货后，或者发货前
            {
                return BadRequest();
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

        /// <summary>
        /// 删除订单记录
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
            if (order.Status != 4 && order.Status != 3)
            {
                return Ok("订单当前状态不支持删除！");
            }

            await _repositoryWrapper.OrderRepository.DeleteAsync(order);
            if (!await _repositoryWrapper.OrderRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok("删除成功！");
        }

    }
}