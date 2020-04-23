using Store.Service.Carts;
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
     public interface IRepositoryWrapper
    {
        IProductRepository ProductRepository { get; }
        IProduct_CategoryRepository Product_CategoryRepository { get; }
        IUserRepository UserRepository { get; }
        IWishRepository WishRepository { get; }

        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
    }
}
