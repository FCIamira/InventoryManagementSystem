using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMangmentSystem.DAL.Data
{
    public interface IGenericRepo<T>
    {
        public  Task Update(T index);
        public Task Delete(int id);
        public Task Add (T index);
        public T GetByID (int id);
        public IQueryable<T> GetAll();
        //public IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        public void Detach(T Entity);

        public Task  Save();


    }
}