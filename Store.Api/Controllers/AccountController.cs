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
 
using Store.Api.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using System.Text;

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
        private readonly SecurityConfigOptions _securityConfigOptions;

        public AccountController(IHttpClientFactory httpClientFactory,IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache,IOptions<SecurityConfigOptions> option)
        {
            this._httpClient= httpClientFactory.CreateClient("service");
            this._repositoryWrapper = repositoryWrapper;
            this._distributedCache = distributedCache;
            this._securityConfigOptions = option.Value;
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
                else
                {
                    return Ok("服务器错误，请联系管理员！");
                }
            }
            return Ok("请一分钟后尝试！");
        }

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="login">登录对象</param>
        /// <returns></returns>
        [HttpPost("email")]
        public async Task<IActionResult> EmailLoginAsync(User_EmailLoginDto login)
        {
            var user = await _repositoryWrapper.UserRepository.EmailLoginAsync(login.Email, login.Password);
            if (user == null)
            {
                return NotFound();
            }

            List<Claim> claimList = new List<Claim>
            {           
                new Claim(JwtClaimTypes.Name,user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,user.NickName),
                new Claim(JwtClaimTypes.Email,user.Email )    
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityConfigOptions.Key));

            SigningCredentials sig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                   issuer: _securityConfigOptions.Issuer,
                   audience: _securityConfigOptions.Audience,
                   claims: claimList,
                   signingCredentials: sig,
                   expires: DateTime.Now.AddMinutes(120)
                 ); ;
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwtToken)}); ;
        }
    }
}