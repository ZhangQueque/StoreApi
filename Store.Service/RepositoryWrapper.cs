using Store.Data;
using Store.Service.Carts;
using Store.Service.Commons;
using Store.Service.Orders;
using Store.Service.Product_Categories;
using Store.Service.Products;
using Store.Service.Users;
using Store.Service.Wishs;
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

        public IUserRepository UserRepository => new UserRepository(context);

        public IWishRepository WishRepository => new WishRepository(context);

        public ICartRepository CartRepository => new CartRepository(context);

        public IOrderRepository OrderRepository => new OrderRepository(context);

        public ICommonRepository<object, int> CommonRepository => new CommonRepository<object,int>(context);
    }
}
