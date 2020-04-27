using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace Store.Service.User_Roles
{
    public class User_RoleRepository : RepositoryBase<User_Role, int>, IUser_RoleRepository
    {

        public User_RoleRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public async Task<User_Role> GetUser_RoleByUserIdAsync(int userId)
        {
            return await context.Set<User_Role>().FirstOrDefaultAsync(m=>m.UserId==userId);
                 
        }
    }
}
