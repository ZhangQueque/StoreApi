﻿using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;
using Store.Dto;

namespace Store.Service.Wishs
{
    /// <summary>
    /// 商品收藏仓储接口
    /// </summary>
    public interface IWishRepository:IRepositoryBaseT<Wish>,IRepositoryBaseTId<Wish, int>
    {
        /// <summary>
        /// 获取用户对应的收藏
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<IEnumerable<WishDto>> GetWishDtosAsync(int userId);


        Task<bool> IsExistProductInWishAsync(int productId);
    }
}
