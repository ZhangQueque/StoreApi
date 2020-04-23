using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;
using Store.Dto;
namespace Store.Service.Orders
{
    /// <summary>
    /// 商品订单仓储接口
    /// </summary>
    public interface IOrderRepository : IRepositoryBaseT<Order>, IRepositoryBaseTId<Order, int>
    {
        /// <summary>
        /// 获取用户对应的订单
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<IEnumerable<OrderDto>> GetOrdersAsync(int userId);
    }
}
