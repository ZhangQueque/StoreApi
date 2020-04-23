using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;
using Store.Dto;
namespace Store.Service.Carts
{
    /// <summary>
    /// 购物车仓储接口
    /// </summary>
    public interface ICartRepository : IRepositoryBaseT<Cart>, IRepositoryBaseTId<Cart, int>
    {

        /// <summary>
        /// 获取用户对应的购物车
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<IEnumerable<CartDto>> GetCartsAsync(int userId);
    }
}
