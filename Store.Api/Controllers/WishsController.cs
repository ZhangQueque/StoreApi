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
using Store.Core.Pages;
using Store.Data;
using Microsoft.EntityFrameworkCore;

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
        private readonly StoreDbContext _context;

        public WishsController(IRepositoryWrapper repositoryWrapper, IMapper mapper, StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
            this._context = context;

        }

        /// <summary>
        /// 用户对应的收藏列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PageList<WishDto>>> GetWishDtosAsync(int index,int size)
        {
            int userId =Convert.ToInt32( User.Identity.Name);
            var data = await _repositoryWrapper.WishRepository.GetWishDtosAsync(userId);
            PageList<WishDto> pageList =await PageList<WishDto>.CreatePageList(data.AsQueryable(), index, size);

            return pageList;
        }


        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="wish">收藏对象</param>
        /// <returns></returns>
       [HttpPost]
        public async Task<IActionResult> AddWishAsync([FromBody]Wish wish)
        {
             if (await _repositoryWrapper.WishRepository.IsExistProductInWishAsync(wish.ProductId))
            {
                return Ok(new { Code=1});
            }
            wish.CreateTime = DateTime.Now;
            wish.UserId = Convert.ToInt32(User.Identity.Name);
            await _repositoryWrapper.WishRepository.AddAsync(wish);
            if (!await _repositoryWrapper.WishRepository.SaveAsync())
            {
                return BadRequest();
            }
            var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "Wish");
            commonData.Value = commonData.Value + 1;
            _context.CommonDatas.Update(commonData);
            await _context.SaveChangesAsync();

            return Ok(new { Code = 0 });
        }


        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="id">收藏主键</param>
        /// <returns></returns>
        [HttpGet("delete/{id}")]
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
            var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "Wish");
            commonData.Value = commonData.Value - 1;
            _context.CommonDatas.Update(commonData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}