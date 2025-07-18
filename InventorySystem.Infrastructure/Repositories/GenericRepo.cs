﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using InventorySystem.Domain.Common;
namespace InventorySystem.Infrastructure.Repositories
{


   
        public class GenericRepo<T, TId> : IGenericRepo<T, TId> where T : BaseModel<TId>
        {
            private readonly ApplicationContext context;
            private readonly DbSet<T> _dbSet;

            public GenericRepo(ApplicationContext _context)
            {
                context = _context;
                _dbSet = context.Set<T>();
            }

            public async Task Add(T obj)
            {
                await _dbSet.AddAsync(obj);
            }

            public async Task<IEnumerable<T>> GetAll()
            {
                return await _dbSet.ToListAsync();
            }

            public async Task<T?> GetById(TId id)
            {
                return await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));
            }


            public async Task Remove(TId id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                }
            }
            public async Task<bool> RemoveByExpression(Expression<Func<T, bool>> predicate)
            {
                var entity = await _dbSet.FirstOrDefaultAsync(predicate);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    return true;
                }
                return false;
            }

            public async Task Update(TId id, T obj)
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(obj);
                }
            }

            public IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> expression)
            {
                return _dbSet.Where(expression);
            }

            public async Task SaveAsync()
            {
                await context.SaveChangesAsync();
            }


        }



    }

