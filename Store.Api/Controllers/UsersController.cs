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

        public UsersController(IRepositoryWrapper repositoryWrapper, IMapper mapper,IDistributedCache distributedCache)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// 获取公共数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonDto> GetCommonDtoAsync()
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            CommonDto common = new CommonDto();
       
            byte[] bytes =await distributedCache.GetAsync($"{userId}_CommonMsg");
            if (bytes==null)
            {
                common.WishCount = (await _repositoryWrapper.WishRepository.GetWishDtosAsync(userId)).Count();
                common.CartCount = (await _repositoryWrapper.CartRepository.GetCartDtosAsync(userId)).Count();
                common.NickName = User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value;


                string json = JsonSerializer.Serialize<CommonDto>(common);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5)
                };
                await distributedCache.SetAsync($"{userId}_CommonMsg", Encoding.UTF8.GetBytes(json), options);
                return common;
            }
            common= JsonSerializer.Deserialize<CommonDto>(Encoding.UTF8.GetString(bytes));
            return common;
        }
       
    }
}