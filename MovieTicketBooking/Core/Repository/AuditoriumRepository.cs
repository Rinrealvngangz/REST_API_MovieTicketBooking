using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using Utilities.Exceptions;
namespace Core.Repository
{
   public class AuditoriumRepository : GenericRepository<Auditorium> , IAuditoriumRepository
    {
        public AuditoriumRepository(AppDbContext dbContext) :base(dbContext)
        {
          
        }

        public override async Task<Auditorium> AddAsync(Auditorium item)
        {
            var exists =  _dbSet.Where(x => x.Name == item.Name).FirstOrDefault();
            if (exists != null) throw new MovieTicketBookingExceptions("Name exists");
               await _dbSet.AddAsync(item);
            return item;
        }

           
        public override async Task<bool> UpdateAsync(string id, Auditorium item)
        {
           
            var exists = await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (exists == null) throw new MovieTicketBookingExceptions("id not exists");
            var updateItem = exists with
            {
                Name = item.Name,
                Capacity = item.Capacity
            };
            _dbSet.Update(updateItem);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (exists == null) throw new MovieTicketBookingExceptions("id not exists");
               _dbSet.Remove(exists);
            return true;
        }

        public override async Task<IEnumerable<Auditorium>> GetAllAsync()
        {
            // Explicit Loading

            //var list = await _dbContext.Auditoriums.AsTracking().ToListAsync();
            //foreach (var item in list)
            //{
            //  await _dbContext.Entry(item).Collection(x => x.Rows).LoadAsync();
            //}

            var items = await _dbContext.Auditoriums.Include(x => x.Rows).ToListAsync();
                return items;
        }




    }
}
