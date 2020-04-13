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
    public class Product_CategoryRepository : RepositoryBase<Product_Category, int>, IProduct_CategoryRepository
    {
        private readonly DbContext context;

        public Product_CategoryRepository(DbContext context) : base(context)
        {
            this.context = context;
        }

        public  Task<IEnumerable<Product_CategoryDto>> GetTreeProduct_CategoryDtoes(int pId)
        {
            return Task.FromResult(BindTree(pId));
        }

        private IEnumerable<Product_CategoryDto> BindTree(int pId)
        {
            var list =  context.Set<Product_Category>().Where(m=>m.PId==pId).Select(m=> new Product_CategoryDto { 
                 Id=m.Id,
                 Title=m.Title,
                 CreateTime=m.CreateTime,
                 PId = m.PId,
                 SortId =m.SortId,
                 Status=m.Status
            }).ToList();
            foreach (var item in list)
            {
                item.ChildList = BindTree(item.Id).ToList();
            }
            return list;
        }
        public async Task<bool> IsExistProducts(int id)
        {
            var count =await context.Set<Product>().CountAsync(m => m.Product_CategoryId == id);
            return count > 0;
        }
    }
}
