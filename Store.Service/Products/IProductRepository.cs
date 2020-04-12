using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Pages;
using Store.Data.Entities;
using Store.Data.Repository;
namespace Store.Service.Products
{
    public interface IProductRepository:IRepositoryBaseT<Product>,IRepositoryBaseTId<Product,int>
    {
        Task<PageList<Product>> GetPageListsAsync(PageParameters pageParameters, int typeId);
    }
}
