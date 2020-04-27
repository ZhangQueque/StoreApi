using Store.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Service.Commons
{
    public interface ICommonRepository<T,TId>:IRepositoryBaseT<T>,IRepositoryBaseTId<T,TId>
        where T:class
    {
    }
}
