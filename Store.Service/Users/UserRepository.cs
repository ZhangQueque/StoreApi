using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace Store.Service.Users
{
    public class UserRepository:RepositoryBase<UserInfo,int>, IUserRepository
    {
        public UserRepository(DbContext dbContext):base(dbContext)
        {

        }

        public async Task<UserInfo> EmailLoginAsync(string email, string password)
        {
            var user = await context.Set<UserInfo>().Where(m=>m.Status==0).FirstOrDefaultAsync(m => m.Email == email && m.Password == password);

            return user;
        }

        public async Task<bool> IsExistEmailAccountAsync(string email)
        {
            return await context.Set<UserInfo>().Where(m => m.Status == 0).FirstOrDefaultAsync(m => m.Email == email)!=null;
        }

        public async Task<bool> IsExistPhoneAccountAsync(string phone)
        {
            return await context.Set<UserInfo>().Where(m => m.Status == 0).FirstOrDefaultAsync(m => m.Phone == phone) != null;
        }

        public async Task<UserInfo> PhoneLoginAsync(string phone)
        {
            var user = await context.Set<UserInfo>().Where(m => m.Status == 0).FirstOrDefaultAsync(m => m.Phone == phone);

            return user;
        }
    }
}
