using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;

namespace Store.Service.Users
{
    public  interface IUserRepository:IRepositoryBaseT<UserInfo>,IRepositoryBaseTId<UserInfo,int>
    {
        Task<bool> IsExistEmailAccountAsync(string email);
        Task<bool> IsExistPhoneAccountAsync(string phone);
        Task<UserInfo> EmailLoginAsync(string email ,string password);

        Task<UserInfo> PhoneLoginAsync(string phone);
    }
}
