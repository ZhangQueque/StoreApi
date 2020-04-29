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
using System.IO;

namespace Store.Api.Controllers
{
    /// <summary>
    /// 公共控制器     切记 这里不是专注于用户，更像是一个公共的控制器
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

        public UsersController(IRepositoryWrapper repositoryWrapper, IMapper mapper, IDistributedCache distributedCache, StoreDbContext context)
        {
            this._repositoryWrapper = repositoryWrapper;
            this._mapper = mapper;
            this.distributedCache = distributedCache;
            this._context = context;
        }

        /// <summary>
        /// 网站访问量
        /// </summary>
        /// <returns></returns>
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
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10)
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
            common.NickName =(await _repositoryWrapper.UserRepository.GetByIdAsync(userId)).NickName;


            var checkLogin = await _context.CheckLogins.FirstOrDefaultAsync(m => m.UserId == userId);

            string cacheLogin = await distributedCache.GetStringAsync("login_" + userId.ToString());
            if (string.IsNullOrEmpty(cacheLogin))
            {
                checkLogin.Status = 1;
                _context.CheckLogins.Update(checkLogin);
                await _context.SaveChangesAsync();

                LogMessage logMessage = new LogMessage { Content = $" “{User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value}” 退出了！", CreateTime = DateTime.Now };
                await _context.LogMessages.AddAsync(logMessage);
                await _context.SaveChangesAsync();
                return BadRequest();
            }
            if (checkLogin.Status == 1)
            {
                await distributedCache.RemoveAsync("login_" + userId.ToString());
                LogMessage logMessage = new LogMessage { Content = $" “{User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value}” 退出了！", CreateTime = DateTime.Now };
                await _context.LogMessages.AddAsync(logMessage);
                await _context.SaveChangesAsync();
                return BadRequest();
            }

            return common;

            // common= JsonSerializer.Deserialize<CommonDto>(Encoding.UTF8.GetString(bytes));
            //  return common;
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>

        [HttpGet("logout")]
        public async Task<IActionResult> LogOutAsync()
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
                await distributedCache.RemoveAsync("login_"+userId.ToString());

            }
            await _context.SaveChangesAsync();
            LogMessage logMessage = new LogMessage { Content = $" “{User.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.NickName).Value}” 退出了！", CreateTime = DateTime.Now };
            await _context.LogMessages.AddAsync(logMessage);
            await _context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userUpdateDto">用户修改对象</param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UserUpdateAsync([FromForm]UserUpdateDto userUpdateDto)
        {


            int userId = Convert.ToInt32(User.Identity.Name);

            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user.Email == userUpdateDto.Email && user.Phone == userUpdateDto.Phone && user.NickName == userUpdateDto.NickName && user.ShippingAddress == userUpdateDto.ShippingAddress && userUpdateDto.Image == null)
            {
                return Ok(new { code = 2, msg = "未进行任何更改！" });

            }
            else
            {
                if (user.Email != userUpdateDto.Email)
                {
                    if (await _repositoryWrapper.UserRepository.IsExistEmailAccountAsync(userUpdateDto.Email))
                    {
                        return Ok(new { code = 1, msg = "该邮箱已存在，请重新输入！" });
                    }
                    user.Email = userUpdateDto.Email;
                }
                if (user.Phone != userUpdateDto.Phone)
                {
                    if (!string.IsNullOrEmpty(userUpdateDto.Phone))
                    {
                        if (await _repositoryWrapper.UserRepository.IsExistPhoneAccountAsync(userUpdateDto.Phone))
                        {
                            return Ok(new { code = 1, msg = "该手机号已存在，请重新输入！" });
                        }
                    }              
                    user.Phone = userUpdateDto.Phone;
                }
                if (user.NickName != userUpdateDto.NickName)
                {
                    user.NickName = userUpdateDto.NickName;
                }
                if (user.ShippingAddress != userUpdateDto.ShippingAddress)
                {
                    user.ShippingAddress = userUpdateDto.ShippingAddress;
                }
                if (userUpdateDto.Image != null)
                {
                    var extensionPath =  Path.GetExtension(userUpdateDto.Image.FileName);
                    string fileName = userId+"_"+ userId+"_"+ userId + extensionPath;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImg", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await userUpdateDto.Image.CopyToAsync(stream);
                    }
                    string imgurl = Request.Scheme + "://" + Request.Host;
                    user.Image = imgurl + "/UserImg/" + fileName;
                }

                await _repositoryWrapper.UserRepository.UpdateAsync(user);

                if (!await _repositoryWrapper.UserRepository.SaveAsync())
                {
                    return BadRequest();
                }//https://localhost:5001/
                return Ok(new { code = 0, msg = "更新成功！" });
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updatePasswordDto">修改密码对象</param>
        /// <returns></returns>
        [HttpPost("pwd")]
        public async Task<IActionResult> UserUpdatePasswordAsync([FromBody]UpdatePasswordDto  updatePasswordDto) 
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user.Password!= updatePasswordDto.OldPassword)
            {
                return Ok(new { code=1,msg="原密码不正确！"});
            }

            user.Password = updatePasswordDto.NewPassword;

            await _repositoryWrapper.UserRepository.UpdateAsync(user);
            if (!await _repositoryWrapper.UserRepository.SaveAsync())
            {
                return BadRequest();
            }

            return Ok(new { code = 0, msg = "密码修改成功！" });
        }


        /// <summary>
        /// 取消手机绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("updatePhone")]
        public async Task<IActionResult> UpdatePhone() 
        {
            int userId = Convert.ToInt32(User.Identity.Name);

            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);
            user.Phone = "";
            await _repositoryWrapper.UserRepository.UpdateAsync(user);
            if (!await _repositoryWrapper.UserRepository.SaveAsync())
            {
                return BadRequest();
            }
            return Ok(new { code = 0, msg = "取消绑定成功！" });
        }

 

    }
}