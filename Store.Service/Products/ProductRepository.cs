using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.Pages;
using Store.Data.Entities;
using Store.Data.Repository;
using System.Linq;
namespace Store.Service.Products
{
    /// <summary>
    /// 商品仓储类
    /// </summary>
    public class ProductRepository: RepositoryBase<Product,int>,IProductRepository
    {
        private readonly DbContext context;

        public ProductRepository(DbContext context):base(context)
        {
            this.context = context;
        }

        public Task<IEnumerable<Product>> GetNewProducts()
        {
            return Task.FromResult( context.Set<Product>().OrderByDescending(m=>m.CreateTime).Take(8).AsEnumerable());
        }

        public async  Task<PageList<Product>> GetPageListsAsync(PageParameters pageParameters, int typeId)
        {
            IQueryable<Product> source = context.Set<Product>().Where(m=>m.Product_CategoryId==typeId);
            if (!string.IsNullOrEmpty(pageParameters.Name))
            {
                source = source.Where(m=>m.Title.Contains(pageParameters.Name));
            }

            //价格区间
            if (pageParameters.BottomPrice!=0 && pageParameters.TopPrice!=0)
            {
                source = source.Where(m => m.Price > pageParameters.BottomPrice && m.Price < pageParameters.TopPrice);
            } //价格区间

            //价格排序
            if (pageParameters.IsPriceSort!=null)
            {
                if ((bool)pageParameters.IsPriceSort) //价格升序
                {
                    source = source.OrderBy<Product, decimal>(m => m.Price);
                }
                else //价格倒序
                {
                    source = source.OrderByDescending<Product, decimal>(m => m.Price);
                }
              
            }//价格排序


            //销量排序
            if (pageParameters.IsPurchaseSort != null)
            {
                if ((bool)pageParameters.IsPurchaseSort) //销量升序
                {
                    source = source.OrderBy<Product, int>(m => m.Purchase);
                }
                else //销量倒序
                {
                    source = source.OrderByDescending<Product, int>(m => m.Purchase);
                }

            }//销量排序

            //日期排序
            if (pageParameters.IsTimeSort != null)
            {
                if ((bool)pageParameters.IsTimeSort) //日期升序
                {
                    source = source.OrderBy<Product, DateTime>(m => m.CreateTime);
                }
                else //日期倒序
                {
                    source = source.OrderByDescending<Product, DateTime>(m => m.CreateTime);
                }

            }//日期排序         
            return await PageList<Product>.CreateLayuiList(source,pageParameters.PageIndex,pageParameters.PageSize); ;
        }

        public Task<IEnumerable<Product>> GetPageViewTopProducts()
        {
            return Task.FromResult(context.Set<Product>().OrderByDescending(m =>m.PageView).Take(8).AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetShopTopProducts()
        {
            return Task.FromResult(context.Set<Product>().OrderByDescending(m => m.Purchase).Take(8).AsEnumerable());
        }
    }
}
