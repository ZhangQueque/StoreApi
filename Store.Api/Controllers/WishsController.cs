using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Dto;
using Store.Service;
using Microsoft.AspNetCore.Authorization;
using Store.Data.Entities;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 商品收藏
    /// </summary>
    [Route("api/wishs")]
    [ApiController]
    [Authorize]
    public class WishsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public WishsController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
        }

        /// <summary>
        /// 用户对应的收藏列表
        /// </summary>
        /// <returns></returns>
       [HttpGet]
        public async Task<IEnumerable<WishDto>> GetWishDtosAsync()
        {
            int userId =Convert.ToInt32( User.Identity.Name);
            var data = await _repositoryWrapper.WishRepository.GetWishDtosAsync(userId);

            return data;
        }


        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="wish">收藏对象</param>
        /// <returns></returns>
       [HttpPost]
        public async Task<IActionResult> AddWishAsync(Wish wish)
        {
            wish.Createtime = DateTime.Now;
            await _repositoryWrapper.WishRepository.AddAsync(wish);
            if (!await _repositoryWrapper.WishRepository.SaveAsync())
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetWishDtosAsync),null, wish);
        }


        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="id">收藏主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishAsync(int id)
        {
            var wish =await _repositoryWrapper.WishRepository.GetByIdAsync(id);
            if (wish==null)
            {
                return NotFound();
            }
            await _repositoryWrapper.WishRepository.DeleteAsync(wish);
            if (!await _repositoryWrapper.WishRepository.SaveAsync())
            {
                return BadRequest();
            }
            return NoContent();
        }

    }
}