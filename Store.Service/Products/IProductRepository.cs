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
        /// 最新商品
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetNewProducts();

        /// <summary>
        /// 销量最高
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetShopTopProducts();

        /// <summary>
        /// 随机商品
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetPageViewTopProducts();
    }
}
