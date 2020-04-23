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

namespace Store.Service.Wishs
{
    /// <summary>
    /// 商品收藏仓储
    /// </summary>
    public class WishRepository : RepositoryBase<Wish, int>, IWishRepository
    {
        public WishRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<WishDto>> GetWishDtosAsync(int userId)
        {
            //var wishList = context.Set<Wish>();        //收藏表
            //var userList = context.Set<UserInfo>();    //用户
            //var productList = context.Set<Product>();  //商品

            //IEnumerable<WishDto> data = wishList
            //    .Join<Wish, UserInfo, int, WishDto>(userList, w => w.UserId, u => u.Id, (w, u) => new WishDto
            //    {   //先和用户表联查    返回了收藏表的Dto(数据传输对象) （wish和user联查）
            //        Createtime = w.Createtime,
            //        Id = w.Id,
                    
            //        ProductId=w.ProductId,
            //        UserId = u.Id,
            //        UserName = u.NickName,             
            //    })
            //    .Join<WishDto, Product, int, WishDto>(productList, w => w.ProductId, p => p.Id, (w, p) => new WishDto
            //    {
            //        //在继续和商品表联查  （wishdto  和 product  联查）
            //        ProductId = p.Id,
            //        Title = p.Title,
            //        Price = p.Price,
            //        Pictrue = p.Pictrue,
            //        ShortDescribe = p.ShortDescribe,                         
                    
            //        Id = w.Id,
            //        UserId = w.UserId,
            //        UserName =w.UserName,
            //        Createtime = w.Createtime,
            //    });

            var list = context.Set<Wish>().Join(context.Set<UserInfo>(), w => w.UserId, u => u.Id, (w, u) => new WishDto
            {
                CreateTime = w.CreateTime,
                Id = w.Id,

                ProductId = w.ProductId,
                UserId = u.Id,
                UserName = u.NickName,
            }).Join(context.Set<Product>(),w=>w.ProductId,p=>p.Id,(w,p)=> new WishDto 
            {
                //在继续和商品表联查  （wishdto  和 product  联查）
                ProductId = p.Id,
                Title = p.Title,
                Price = p.Price,
                Pictrue = p.Pictrue,
                ShortDescribe = p.ShortDescribe,

                Id = w.Id,
                UserId = w.UserId,
                UserName = w.UserName,
                CreateTime = w.CreateTime,
            }).Where(m=>m.UserId == userId);

            return await list.ToListAsync();
        }
    }
}
