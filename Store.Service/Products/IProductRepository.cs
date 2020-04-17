using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Pages;
using Store.Data.Entities;
using Store.Data.Repository;
namespace Store.Service.Products
{
    /// <summary>
    /// 商品仓储接口
    /// </summary>
    public interface IProductRepository:IRepositoryBaseT<Product>,IRepositoryBaseTId<Product,int>
    {
        Task<PageList<Product>> GetPageListsAsync(PageParameters pageParameters, int typeId);

        /// <summary>
        /// 根据主键获取商品
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Product> GetProductById(int id);

        /// <summary>
        /// 最新商品
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetNewProductsAsync();

        /// <summary>
        /// 销量最高
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetShopTopProductsAsync();

        /// <summary>
        /// 推荐商品
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetPageViewTopProductsAsync();
    }
}
