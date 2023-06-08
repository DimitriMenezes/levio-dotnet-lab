using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Abstract
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> GetById(int id);
        Task<IQueryable<TEntity>> GetAll();
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(int id);
        Task<IQueryable<TEntity>> Include();        
    }
}
