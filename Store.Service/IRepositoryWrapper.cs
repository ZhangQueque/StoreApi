using Store.Service.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Service
{
     public interface IRepositoryWrapper
    {
        IProductRepository ProductRepository { get; }
    }
}
