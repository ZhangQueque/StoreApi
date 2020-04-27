using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;


namespace Store.Service.User_Roles
{
    public interface IUser_RoleRepository : IRepositoryBaseT<User_Role>, IRepositoryBaseTId<User_Role, int>
    {
        Task<User_Role> GetUser_RoleByUserIdAsync(int userId);
    }
}
