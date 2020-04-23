using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using System.Threading.Tasks;
namespace Store.Service.Wishs
{
    public interface IWishRepository:IRepositoryBaseT<Wish>,IRepositoryBaseTId<Wish, int>
    {

    }
}
