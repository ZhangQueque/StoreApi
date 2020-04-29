using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Store.Dto;

using Microsoft.EntityFrameworkCore.Internal;

namespace Store.Service.Orders
{
    /// <summary>
    /// 订单仓储
    /// </summary>
    public class OrderRepository : RepositoryBase<Order, int>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int userId)
        {
            var list = context.Set<Order>().Where(m=>m.UserId== userId).Join(context.Set<UserInfo>(), o => o.UserId, u => u.Id, (o, u) => new OrderDto
            {
                UserId = o.UserId,
                Count = o.Count,
                CreateTime = o.CreateTime,
                TotalPrices = o.TotalPrices,
                Id = o.Id,
                ProductId = o.ProductId,
                Status = o.Status,
                ShippingAddress = u.ShippingAddress,
                 Size=o.Size

            }).Join(context.Set<Product>(), o => o.ProductId, p => p.Id, (o, p) => new OrderDto
            {
                Title = p.Title,
                Pictrue = p.Pictrue,
                Price = p.Price,
                ShortDescribe = p.ShortDescribe,


                Size = o.Size,
                UserId = o.UserId,
                Count = o.Count,
                CreateTime = o.CreateTime,
                TotalPrices = o.TotalPrices,
                Id = o.Id,
                ProductId = o.ProductId,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
            });
            return await list.ToListAsync();

        }



        public async Task<IEnumerable<OrderDto>> GetOrdersAllAsync()
        {
            var list = context.Set<Order>().Join(context.Set<UserInfo>(), o => o.UserId, u => u.Id, (o, u) => new OrderDto
            {
                UserId = o.UserId,
                Count = o.Count,
                CreateTime = o.CreateTime,
                TotalPrices = o.TotalPrices,
                Id = o.Id,
                ProductId = o.ProductId,
                Status = o.Status,
                ShippingAddress = u.ShippingAddress,
                Size = o.Size

            }).Join(context.Set<Product>(), o => o.ProductId, p => p.Id, (o, p) => new OrderDto
            {
                Title = p.Title,
                Pictrue = p.Pictrue,
                Price = p.Price,
                ShortDescribe = p.ShortDescribe,


                Size = o.Size,
                UserId = o.UserId,
                Count = o.Count,
                CreateTime = o.CreateTime,
                TotalPrices = o.TotalPrices,
                Id = o.Id,
                ProductId = o.ProductId,
                Status = o.Status,
                ShippingAddress = o.ShippingAddress,
            });
            return await list.ToListAsync();

        }
    }
}
