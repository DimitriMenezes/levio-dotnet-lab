using Microsoft.EntityFrameworkCore;
using StockMarket.Data.Abstract;
using StockMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Concrete
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        internal readonly DbContext _context;
        internal DbSet<TEntity> _dbSet;

        protected BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task Delete(int id)
        {
            var entity = _dbSet.Find(id);            
            _dbSet.Remove(entity);            
        }

        public async Task<IQueryable<TEntity>> GetAll()
        {
            return await Include();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            await _dbSet.AddAsync(entity);            
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;            
            return entity;
        }
        public async Task<IQueryable<TEntity>> Include()
        {
            return _dbSet;
        }     
    }
}
