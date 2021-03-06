using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
namespace Core.IRepository
{
   public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<T> AddAsync(T item);

        Task<bool> UpdateAsync(string id ,T item);

        Task<bool> DeleteAsync(Guid id);
    }
}
