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
using AutoMapper;
using Store.Data.Entities;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using Store.Data;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;
        private readonly SecurityConfigOptions _securityConfigOptions;
        private readonly StoreDbContext _context;

        public AccountController(IHttpClientFactory httpClientFactory, IRepositoryWrapper repositoryWrapper, IDistributedCache distributedCache, IOptions<SecurityConfigOptions> option, IMapper mapper, StoreDbContext context)
        {
            this._httpClient = httpClientFactory.CreateClient("service");
            this._repositoryWrapper = repositoryWrapper;
            this._distributedCache = distributedCache;
            this._mapper = mapper;
            this._securityConfigOptions = option.Value;
            this._context = context;

        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCodeAsync(string phone)
        {
            Regex regex = new Regex(@"^[1](([3][0-9])|([4][5-9])|([5][0-3,5-9])|([6][5,6])|([7][0-8])|([8][0-9])|([9][1,8,9]))[0-9]{8}$");
            if (!regex.IsMatch(phone))
            {
                return Ok("请输入正确的手机号！");
            }
            string code = await _distributedCache.GetStringAsync(phone);
            if (string.IsNullOrEmpty(code))
            {
                code = new VerifyCode().CreateVCode(6);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                };
                DistributedCacheEntryOptions options2 = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5)
                };
                await _distributedCache.SetStringAsync(phone, code, options);
                await _distributedCache.SetStringAsync(phone + "_code", code, options2);
                SendPhoneModel sendPhoneModel = new SendPhoneModel
                {
                    Phone = phone,
                    Code = code,
                    Key = "138345"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/service/sms", sendPhoneModel);
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
        public async Task<IActionResult> EmailLoginAsync([FromBody]User_EmailLoginDto login)
        {
            var user = await _repositoryWrapper.UserRepository.EmailLoginAsync(login.Email, login.Password);
            if (user == null)
            {
                return NotFound();
            }
            var roleInfo = await _repositoryWrapper.User_RoleRepository.GetUser_RoleByUserIdAsync(user.Id);

            List<Claim> claimList = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name,user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,user.NickName),
                new Claim(JwtClaimTypes.Email,user.Email ),
                new Claim(JwtClaimTypes.Role,roleInfo.RoleName) 
                //new Claim(JwtClaimTypes.Role,"管理员"),
                //new Claim(JwtClaimTypes.Role,"游民")


            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityConfigOptions.Key));

            SigningCredentials sig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                   issuer: _securityConfigOptions.Issuer,
                   audience: _securityConfigOptions.Audience,
                   claims: claimList,
                   signingCredentials: sig,
                   expires: DateTime.Now.AddMinutes(220)
                 ) ;

            LogMessage logMessage = new LogMessage { Content=$" \"{user.NickName}\" 登陆了！",CreateTime=DateTime.Now};
            await _context.LogMessages.AddAsync(logMessage);
            await _context.SaveChangesAsync();


            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m=>m.UserId==user.Id);
            if (checkLogin == null)
            {
                CheckLogin check  = new CheckLogin { UserId = user.Id, Status = 0 };
                await _context.CheckLogins.AddAsync(check);
            }
            else {
                checkLogin.Status = 0;
                  _context.CheckLogins.Update(checkLogin);
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "登录成功！", token = new JwtSecurityTokenHandler().WriteToken(jwtToken) }); ;
        }


        /// <summary>
        /// 邮箱注册，返回token         
        /// </summary>
        /// <param name="register">注册对象</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> EmailRegisterAsync([FromBody]User_EmailLoginDto register)
        {
            if (await _repositoryWrapper.UserRepository.IsExistEmailAccountAsync(register.Email))
            {
                return Ok(new { code = 1, msg = "账号已存在！" });
            }
            var user = _mapper.Map<UserInfo>(register);
            user.IsExistEmail = 1;
            user.CreateId = 0;
            user.CreateTime = DateTime.Now;
            user.NickName = register.Email;
            user.UpdateTime = DateTime.Now;

            await _repositoryWrapper.UserRepository.AddAsync(user);
            if (!await _repositoryWrapper.UserRepository.SaveAsync())
            {
                return BadRequest();
            }
            User_Role user_Role = new User_Role {  RoleId=2, RoleName="帮众", UserId = user.Id, UserEmail=user.Email};

            await _repositoryWrapper.User_RoleRepository.AddAsync(user_Role);
            await _repositoryWrapper.User_RoleRepository.SaveAsync();

            var roleInfo = await _repositoryWrapper.User_RoleRepository.GetUser_RoleByUserIdAsync(user.Id);
            
            List<Claim> claimList = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name,user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,user.NickName),

                new Claim(JwtClaimTypes.Email,user.Email ),
                new Claim(JwtClaimTypes.Role, roleInfo.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityConfigOptions.Key));

            SigningCredentials sig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                   issuer: _securityConfigOptions.Issuer,
                   audience: _securityConfigOptions.Audience,
                   claims: claimList,
                   signingCredentials: sig,
                   expires: DateTime.Now.AddMinutes(220)
                 );
            var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "User");
            commonData.Value = commonData.Value + 1;
            _context.CommonDatas.Update(commonData);
            await _context.SaveChangesAsync();


            LogMessage logMessage = new LogMessage { Content = $" \"{user.NickName}\" 登陆了！", CreateTime = DateTime.Now };
            await _context.LogMessages.AddAsync(logMessage);
            await _context.SaveChangesAsync();

            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == user.Id);
            if (checkLogin == null)
            {
                CheckLogin check = new CheckLogin { UserId = user.Id, Status = 0 };
                await _context.CheckLogins.AddAsync(check);
            }
            else
            {
                checkLogin.Status = 0;
                _context.CheckLogins.Update(checkLogin);
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "注册成功！", token = new JwtSecurityTokenHandler().WriteToken(jwtToken) });
        }

        /// <summary>
        /// 手机号登录验证
        /// </summary>
        /// <param name="login">登录对象</param>
        /// <returns></returns>
        [HttpPost("phone")]
        public async Task<IActionResult> PhoneLoginAsync([FromBody] User_PhoneLoginDto login)
        {
            UserInfo user = null;

            //手机号存在时
            if (await _repositoryWrapper.UserRepository.IsExistPhoneAccountAsync(login.Phone))
            {
                string codePhone = await _distributedCache.GetStringAsync(login.Phone + "_code");
                if (string.IsNullOrEmpty(codePhone))
                {
                    return Ok(new { code = 1, msg = "验证码已过期！" });
                }
                if (codePhone != login.Code)
                {
                    return Ok(new { code = 1, msg = "不正确已过期！" });

                }
                user = await _repositoryWrapper.UserRepository.PhoneLoginAsync(login.Phone);
                var roleInfo2 = await _repositoryWrapper.User_RoleRepository.GetUser_RoleByUserIdAsync(user.Id);
                List<Claim> claimList = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name,user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,user.NickName),
                 new Claim(JwtClaimTypes.PhoneNumber,user.Phone ),
                 new Claim(JwtClaimTypes.Role, roleInfo2.RoleName)

            };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityConfigOptions.Key));

                SigningCredentials sig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var jwtToken = new JwtSecurityToken(
                       issuer: _securityConfigOptions.Issuer,
                       audience: _securityConfigOptions.Audience,
                       claims: claimList,
                       signingCredentials: sig,
                       expires: DateTime.Now.AddMinutes(220)
                     );

                LogMessage logMessage2 = new LogMessage { Content = $" \"{user.NickName}\" 登陆了！", CreateTime = DateTime.Now };
                await _context.LogMessages.AddAsync(logMessage2);
                await _context.SaveChangesAsync();

                var checkLogin2 = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == user.Id);
                if (checkLogin2 == null)
                {
                    CheckLogin check2 = new CheckLogin { UserId = user.Id, Status = 0 };
                    await _context.CheckLogins.AddAsync(check2);
                }
                else
                {
                    checkLogin2.Status = 0;
                    _context.CheckLogins.Update(checkLogin2);
                }
                await _context.SaveChangesAsync();

                return Ok(new { code = 0, msg = "验证成功！", token = new JwtSecurityTokenHandler().WriteToken(jwtToken) });
            }

            string codePhone2 = await _distributedCache.GetStringAsync(login.Phone + "_code");
            if (string.IsNullOrEmpty(codePhone2))
            {
                return Ok(new { code = 1, msg = "验证码已过期！" });
            }
            if (codePhone2 != login.Code)
            {
                return Ok(new { code = 1, msg = "不正确已过期！" });

            }

            //手机号不存在时
            user = _mapper.Map<UserInfo>(login);
            user.IsExistPhone = 1;
            user.CreateId = 0;
            user.CreateTime = DateTime.Now;
            user.NickName = login.Phone;
            user.UpdateTime = DateTime.Now;

            await _repositoryWrapper.UserRepository.AddAsync(user);
            if (!await _repositoryWrapper.UserRepository.SaveAsync())
            {
                return BadRequest();
            }

            User_Role user_Role = new User_Role { RoleId = 2, RoleName = "帮众", UserId = user.Id, UserEmail = user.Phone };

            await _repositoryWrapper.User_RoleRepository.AddAsync(user_Role);
            await _repositoryWrapper.User_RoleRepository.SaveAsync();


            var roleInfo = await _repositoryWrapper.User_RoleRepository.GetUser_RoleByUserIdAsync(user.Id);


            List<Claim> claimList2 = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name,user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,user.NickName),
                new Claim(JwtClaimTypes.PhoneNumber,user.Phone ),
                new Claim(JwtClaimTypes.Role, roleInfo.RoleName)
            };

            var key2 = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityConfigOptions.Key));

            SigningCredentials sig2 = new SigningCredentials(key2, SecurityAlgorithms.HmacSha256Signature);

            var jwtToken2 = new JwtSecurityToken(
                   issuer: _securityConfigOptions.Issuer,
                   audience: _securityConfigOptions.Audience,
                   claims: claimList2,
                   signingCredentials: sig2,
                   expires: DateTime.Now.AddMinutes(220)
                 );

            var commonData = await _context.CommonDatas.FirstOrDefaultAsync(m => m.Type == "User");
            commonData.Value = commonData.Value + 1;
            _context.CommonDatas.Update(commonData);
            await _context.SaveChangesAsync();

            LogMessage logMessage = new LogMessage { Content = $" \"{user.NickName}\" 登陆了！", CreateTime = DateTime.Now };
            await _context.LogMessages.AddAsync(logMessage);
            await _context.SaveChangesAsync();


            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == user.Id);
            if (checkLogin == null)
            {
                CheckLogin check = new CheckLogin { UserId = user.Id, Status = 0 };
                await _context.CheckLogins.AddAsync(check);
            }
            else
            {
                checkLogin.Status = 0;
                _context.CheckLogins.Update(checkLogin);
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = 0, msg = "验证成功！", token = new JwtSecurityTokenHandler().WriteToken(jwtToken2) });
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        [HttpGet("password")]
        public async Task<IActionResult> RetrievePasswordAsync(string email)
        {
            Regex regex = new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");
            if (!regex.IsMatch(email))
            {
                return Ok(new { code = 1, msg = "请输入正确的邮箱！" });
            }

            SendEmailModel sendEmailModel = new SendEmailModel
            {
                Title = "重置密码",
                EmailAddress = email,
                Content = "缺缺提醒您：请联系管理员，QQ：3393597524！",
                Key = "138345",
                Addresser = "缺缺",
                Recipients = ""
            };
            var response = await _httpClient.PostAsJsonAsync("/api/service/smtp", new List<SendEmailModel>() { sendEmailModel });
            if (response.IsSuccessStatusCode)
            {
                return Ok("发送成功！");
            }
            else
            {
                return Ok("服务器错误，请联系管理员！");
            }
        }

    }
}