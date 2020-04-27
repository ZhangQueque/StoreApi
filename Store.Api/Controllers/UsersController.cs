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
using IdentityModel;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Net;
using Store.Data;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IDistributedCache distributedCache;
        private readonly StoreDbContext _context;

        public UsersController(IRepositoryWrapper repositoryWrapper, IMapper mapper,IDistributedCache distributedCache,StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
            this.distributedCache = distributedCache;
            this._context = context;
        }

        [HttpGet("view")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewAddAsync()
        {
            string ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            string ip = await distributedCache.GetStringAsync($"{ipAddress}_IP4");
            if (string.IsNullOrEmpty(ip))
            {

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(15)
                };
                var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "View");
                commonData.Value = commonData.Value + 1;
                _context.CommonDatas.Update(commonData);
                await _context.SaveChangesAsync();
                await distributedCache.SetStringAsync($"{ipAddress}_IP4", ipAddress, options);
            }
            return Ok();
        }

        /// <summary>
        /// 获取公共数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CommonDto>> GetCommonDtoAsync()
        {
           
            int userId = Convert.ToInt32(User.Identity.Name);

            CommonDto common = new CommonDto();
       
            
            common.WishCount = (await _repositoryWrapper.WishRepository.GetWishDtosAsync(userId)).Count();
            common.CartCount = (await _repositoryWrapper.CartRepository.GetCartDtosAsync(userId)).Count();
            common.NickName = User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value;


            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == userId);
            if (checkLogin.Status==1)
            {
                return BadRequest();
            }

            return common;
          
           // common= JsonSerializer.Deserialize<CommonDto>(Encoding.UTF8.GetString(bytes));
          //  return common;
        }


        [HttpGet("logout")]
         public async Task<IActionResult> LogOut()
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == userId);
            if (checkLogin == null)
            {
                return BadRequest();
            }
            else
            {
                checkLogin.Status = 1;
                _context.CheckLogins.Update(checkLogin);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}