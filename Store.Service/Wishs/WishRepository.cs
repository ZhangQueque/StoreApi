using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Store.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace Store.Service.Wishs
{
    public class WishRepository:RepositoryBase<Wish,int>,IWishRepository
    {
        public WishRepository(DbContext context):base(context)
        {

        }
    }
}
