using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _dbContext;
        protected DbSet<T> _dbSet;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public virtual async Task<T> AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
            return item;
        }

        public virtual Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
           var item = await _dbSet.FindAsync(id);
            return item;
        }

        public virtual Task<bool> UpdateAsync(string id ,T item)
        {
            throw new NotImplementedException();
        }
    }
}
