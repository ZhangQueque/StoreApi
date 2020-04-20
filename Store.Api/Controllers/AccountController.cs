using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Store.Core;
using Store.Service;
using AspNetCore.Http.Extensions;
using Store.Dto;

namespace Store.Api.Controllers
{

    /// <summary>
    /// 账号控制器
    /// </summary>
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IDistributedCache _distributedCache;

        public AccountController(IHttpClientFactory httpClientFactory,IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache)
        {
            this._httpClient= httpClientFactory.CreateClient("service");
            this._repositoryWrapper = repositoryWrapper;
            this._distributedCache = distributedCache;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
       [HttpGet]
        public async Task<IActionResult> GetCode(string phone)
        {
            string code =await _distributedCache.GetStringAsync(phone);
            if (string.IsNullOrEmpty(code))
            {
                code= new VerifyCode().CreateVCode(6);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                };
                DistributedCacheEntryOptions options2 = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5)
                };
                await _distributedCache.SetStringAsync(phone,code,options);
                await _distributedCache.SetStringAsync(phone+code, code, options2);
                SendPhoneModel sendPhoneModel = new SendPhoneModel
                {
                    Phone = phone,
                    Code=code,
                    Key= "138345"
                };

               var response=  await _httpClient.PostAsJsonAsync("/api/service/sms", sendPhoneModel);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("发送成功！");
                }
            }
            return Ok("请一分钟后尝试！");
        }

        [HttpPost("email")]
        public async Task<IActionResult> EmailLoginAsync(User_EmailLoginDto login)
        {
            var user = await _repositoryWrapper.UserRepository.EmailLoginAsync(login.Email,login.Password);
            if (user==null)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}