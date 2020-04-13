using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Store.Data.Entities;
using Store.Data.Repository;
using Store.Dto;

namespace Store.Service.Product_Categories
{
    public interface IProduct_CategoryRepository:IRepositoryBaseT<Product_Category>,IRepositoryBaseTId<Product_Category,int>
    {
        /// <summary>
        /// 检测分类下是否存在商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        Task<bool> IsExistProducts(int id);

        Task<IEnumerable<Product_CategoryDto>> GetTreeProduct_CategoryDtoes(int pId);
    }
}
