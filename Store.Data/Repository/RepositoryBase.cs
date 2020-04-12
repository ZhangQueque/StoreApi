using Microsoft.EntityFrameworkCore;
using Store.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repository
{
    /// <summary>
    /// 仓储父类
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <typeparam name="TId">主键</typeparam>
    public class RepositoryBase<T, TId> : IRepositoryBaseT<T>, IRepositoryBaseTId<T, TId>
        where T:class
    {
        private readonly DbContext context;

        public RepositoryBase(DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T t)
        {
             await context.Set<T>().AddAsync(t);           
        }

        public  Task DeleteAsync(T t)
        {
            context.Set<T>().Remove(t);
            return Task.CompletedTask;
        }

        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.FromResult(context.Set<T>().AsQueryable());
        }

        public async Task<T> GetByIdAsync(TId id)
        {
           return  await context.Set<T>().FindAsync(id);
        }

        public virtual Task<PageList<T>> GetPageListsAsync(PageParameters pageParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistAsync(TId id)
        {
            return await context.Set<T>().FindAsync(id)!=null;
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public Task UpdateAsync(T t)
        {
            context.Set<T>().Update(t);
            return Task.CompletedTask;
        }
    }
}
