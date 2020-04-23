using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;

namespace Store.Service.Users
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public  interface IUserRepository:IRepositoryBaseT<UserInfo>,IRepositoryBaseTId<UserInfo,int>
    {
        /// <summary>
        /// 验证邮箱是否存在
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        Task<bool> IsExistEmailAccountAsync(string email);
        /// <summary>
        /// 验证电话是否存在
        /// </summary>
        /// <param name="phone">电话</param>
        /// <returns></returns>
        Task<bool> IsExistPhoneAccountAsync(string phone);
        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<UserInfo> EmailLoginAsync(string email ,string password);

        /// <summary>
        /// 手机登录
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        Task<UserInfo> PhoneLoginAsync(string phone);
    }
}
