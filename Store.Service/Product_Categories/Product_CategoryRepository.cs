using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Store.Core.Pages;
using Store.Data.Entities;
using Store.Data.Repository;
using System.Linq;
using System.Threading.Tasks;
using Store.Dto;

namespace Store.Service.Product_Categories
{
    /// <summary>
    /// 商品类别仓储类
    /// </summary>
    public class Product_CategoryRepository : RepositoryBase<Product_Category, int>, IProduct_CategoryRepository
    {
        private readonly DbContext context;
        private IEnumerable<Product_CategoryDto> data;
        public Product_CategoryRepository(DbContext context) : base(context)
        {
            this.context = context;
        }

        public  async Task<IEnumerable<Product_CategoryDto>> GetTreeProduct_CategoryDtoesAsync(int pId)
        {
            data = context.Set<Product_Category>().Select(m => new Product_CategoryDto
            {
                Id = m.Id,
                Title = m.Title,
                CreateTime = m.CreateTime,
                PId = m.PId,
                SortId = m.SortId,
                Status = m.Status
            }).ToList();
            return  await BindTreeAsync(pId) ;
        }

        private async Task< IEnumerable<Product_CategoryDto>> BindTreeAsync( int pId)
        {
            var list = data.Where(m => m.PId == pId);

            foreach (var item in list)
            {
                item.ChildList =(await BindTreeAsync(item.Id)).ToList();
                if (item.Id==1)
                {
                    item.ProductList = context.Set<Product>().Where(m=>m.Product_CategoryId==9).OrderByDescending(m => m.CreateTime).Take(2).ToList();
                }
                if (item.Id == 2)
                {
                    item.ProductList = context.Set<Product>().Where(m => m.Product_CategoryId == 14).OrderByDescending(m => m.CreateTime).Take(2).ToList();
                }
                if (item.Id == 3)
                {
                    item.ProductList = context.Set<Product>().Where(m => m.Product_CategoryId == 18).OrderByDescending(m => m.CreateTime).Take(2).ToList();
                }
                if (item.Id == 4)
                {
                    item.ProductList = context.Set<Product>().Where(m => m.Product_CategoryId == 21).OrderByDescending(m => m.CreateTime).Take(2).ToList();
                }       
            }
            return list;
        }
        public async Task<bool> IsExistProductsAsync(int id)
        {
            var count =await context.Set<Product>().CountAsync(m => m.Product_CategoryId == id);
            return count > 0;
        }
    }
}
