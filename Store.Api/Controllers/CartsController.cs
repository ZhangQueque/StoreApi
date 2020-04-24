using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Store.Service;
using AutoMapper;
using Store.Dto;
using Store.Data.Entities;

namespace Store.Api.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public CartsController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
        }

        /// <summary>
        /// 返回用户对应的购物车
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCartsAsync()
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            var data = await _repositoryWrapper.CartRepository.GetCartsAsync(userId);

            return data.ToList();
        }


        /// <summary>
        /// 购物车新增
        /// </summary>
        /// <param name="cart">新增对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCartAsync(Cart cart)
        {
            cart.UserId= Convert.ToInt32(User.Identity.Name); 
            cart.CreateTime = DateTime.Now;
            await _repositoryWrapper.CartRepository.AddAsync(cart);
            if (!await _repositoryWrapper.CartRepository.SaveAsync())
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetCartsAsync), null, cart);
        }

        /// <summary>
        /// 购物车删除
        /// </summary>
        /// <param name="id">购物车主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartAsync(int id)
        {
            var cart = await _repositoryWrapper.CartRepository.GetByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            await _repositoryWrapper.CartRepository.DeleteAsync(cart);
            if (!await _repositoryWrapper.CartRepository.SaveAsync())
            {
                return BadRequest();
            }
            return NoContent();
        }

    }
}