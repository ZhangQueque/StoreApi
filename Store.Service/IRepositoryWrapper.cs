using Store.Service.Product_Categories;
using Store.Service.Products;
using Store.Service.Users;
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
    }
}
