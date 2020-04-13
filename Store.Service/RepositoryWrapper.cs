using Store.Data;
using Store.Service.Product_Categories;
using Store.Service.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Service
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly StoreDbContext context;

        public RepositoryWrapper(StoreDbContext context)
        {
            this.context = context;
        }
        public IProductRepository ProductRepository => new ProductRepository(context);

        public IProduct_CategoryRepository Product_CategoryRepository => new Product_CategoryRepository(context);
    }
}
