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
using System.Threading;
using Store.Api.RedisCache;

namespace Store.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IDistributedCache _distributedCache;
        private readonly RedisCacheHelper _cacheHelper;
        private readonly IMapper _mapper;
        private readonly StoreDbContext _context;

        public AdminController(IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache, RedisCacheHelper cacheHelper, IMapper mapper, StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._distributedCache = distributedCache;
            this._cacheHelper = cacheHelper;
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

            var data = await _context.LogMessages.OrderByDescending(m => m.CreateTime).Take(33).ToListAsync();
            return data;
        }

        /// <summary>
        /// 网站计数
        /// </summary>
        /// <returns></returns>
        [HttpGet("number")]
        public async Task<ActionResult<List<CommonData>>> GetNumberAsync()
        {

            var data = await _context.CommonDatas.ToListAsync();
            return data;
        }

        /// <summary>
        /// 统计图数据
        /// </summary>
        /// <returns></returns>

        [HttpGet("chart")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ChartDto>>> GetChartDataAsync()
        {
            IEnumerable<ChartDto> list = Enumerable.Empty<ChartDto>();
            byte[] bytes = await _distributedCache.GetAsync("chartData");
            if (bytes == null)
            {
                var orders = await _repositoryWrapper.OrderRepository.GetAllAsync();
                List<ChartDto> chartDtos = new List<ChartDto>();

                foreach (var item in orders.Where(m => m.Status != 4).ToList())
                {
                    ChartDto chartDto = new ChartDto();
                    var product = await _repositoryWrapper.ProductRepository.GetByIdAsync(item.ProductId);

                    var cate = await _repositoryWrapper.Product_CategoryRepository.GetByIdAsync(product.Product_CategoryId);
                    if (cate.Id == 30 || cate.Id == 31 || cate.Id == 32)
                    {
                        chartDto.Id = cate.Id;
                        chartDto.Name = cate.Title;
                        chartDto.Count = item.Count;
                        chartDtos.Add(chartDto);
                        continue;
                    }
                    var parentCate = await _repositoryWrapper.Product_CategoryRepository.GetByIdAsync(cate.PId);
                    chartDto.Id = parentCate.Id;
                    chartDto.Name = parentCate.Title;
                    chartDto.Count = item.Count;
                    chartDtos.Add(chartDto);
                }

                list = chartDtos.GroupBy(m => m.Name).Select(g => new ChartDto { Name = g.Key.ToString(), Count = g.Sum(m => m.Count), Id = g.FirstOrDefault().Id }).OrderBy(m => m.Id).ToList();
                await _cacheHelper.SetRedisCacheAsync("chartData", list);
                return list.ToList();

            }
            list = await _cacheHelper.GetRedisCacheAsync<List<ChartDto>>(bytes);

            return list.ToList(); ;
        }

    }
}