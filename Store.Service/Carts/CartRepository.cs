using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Store.Dto;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
namespace Store.Service.Carts
{
    /// <summary>
    /// 购物车仓储
    /// </summary>
    public class CartRepository : RepositoryBase<Cart, int>, ICartRepository
    {
        public CartRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CartDto>> GetCartDtosAsync(int userId)
        {
            var list = context.Set<Cart>().Join(context.Set<UserInfo>(), c => c.UserId, u => u.Id, (c, u) => new CartDto
            {
                CreateTime = c.CreateTime,
                Id = c.Id,
                   Size=c.Size,
                ProductId = c.ProductId,
                UserId = u.Id,
                Count = c.Count
                
            }).Join(context.Set<Product>(), c => c.ProductId, p => p.Id, (c, p) => new CartDto
            {
                //在继续和商品表联查  （wishdto  和 product  联查）
                ProductId = p.Id,
                Title = p.Title,
                Price = p.Price,
                Pictrue = p.Pictrue,
                ShortDescribe = p.ShortDescribe,
                Count = c.Count, 
                Id = c.Id,
                UserId = c.UserId,       
                CreateTime = c.CreateTime,
                Size = c.Size
            }).Where(m => m.UserId == userId);

            return await list.ToListAsync();
        }
    }
}
