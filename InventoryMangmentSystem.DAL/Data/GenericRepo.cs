using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.Data
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        InventoryContext _dbContext;
        public GenericRepo(InventoryContext context)
        {
            _dbContext = context;
        }


        public async Task Update(T entity)
        {
            var entry = _dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
                entry.State = EntityState.Modified;
            }
        }

        //public async Task Delete(int id)
        //{
        //    var entity = await _dbContext.Set<T>().FindAsync(id);
        //    if (entity != null)
        //    {
        //        _dbContext.Set<T>().Remove(entity);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //}
        public async Task Delete(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                var prop = typeof(T).GetProperty("IsDelete");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                    _dbContext.Set<T>().Update(entity);
                }
                else
                {
                    _dbContext.Set<T>().Remove(entity);
                }

                await _dbContext.SaveChangesAsync();
            }
        }


        public T GetByID(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }




        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression);
        }
        public async Task Add(T index)
        {
            await _dbContext.AddAsync(index);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Detach(T Entity)
        {
            var entry = _dbContext.Entry(Entity);
            if (entry.State == EntityState.Detached)
                return;
            entry.State = EntityState.Detached;
        }

    }

}
