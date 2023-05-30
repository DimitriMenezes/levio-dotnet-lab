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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.Email == email);
        }
    }
}
