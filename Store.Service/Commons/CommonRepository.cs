using Microsoft.EntityFrameworkCore;
using Store.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Service.Commons
{
    public class CommonRepository<T,TId>:RepositoryBase<T,TId>, ICommonRepository<T,TId>
        where T:class
    {

        public CommonRepository(DbContext context):base(context)
        {

        }
    }
}
