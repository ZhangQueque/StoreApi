using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Store.Data;
using Store.Data.Entities;
using Store.Service;
using Microsoft.AspNetCore.Authorization;
using Store.Dto;
using Microsoft.EntityFrameworkCore;

namespace Store.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;
        private readonly StoreDbContext _context;

        public AdminController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache, IMapper mapper, StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._distributedCache = distributedCache;
            this._mapper = mapper;
            this._context = context;

        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUserInfoAsync()
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.User_Role = await _repositoryWrapper.User_RoleRepository.GetUser_RoleByUserIdAsync(userId);

            return userDto;
        }

        /// <summary>
        /// 消息日志
        /// </summary>
        /// <returns></returns>
        [HttpGet("logmsg")]
        public async Task<ActionResult<List<LogMessage>>> GetLogMsgAsync()
        {
      
            var data = await _context.LogMessages.OrderByDescending(m => m.CreateTime).Take(10).ToListAsync();
            return data;
        }

    }
}