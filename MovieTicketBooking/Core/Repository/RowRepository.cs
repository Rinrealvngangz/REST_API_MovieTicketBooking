using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
using Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
   public class RowRepository : GenericRepository<Row> , IRowRepository
    {

        public RowRepository(AppDbContext dbContext):base(dbContext)
        {

        }

        public override async Task<Row> AddAsync(Row item)
        {
            var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Number == item.Number && x.AuditoriumId == item.AuditoriumId);
            if (existItem != null) throw new MovieTicketBookingExceptions("Number row has exist");
            await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, Row item)
        {
           var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (existItem is null) throw new  MovieTicketBookingExceptions("id  no exist");
         
            var exitsNumber =  await _dbSet.FirstOrDefaultAsync(x => x.Number == existItem.Number && x.Id != existItem.Id);
            if(exitsNumber is not null) throw new MovieTicketBookingExceptions("Number row has exist");

            var existNumberInAudi = await _dbSet.FirstOrDefaultAsync(x => x.Number == item.Number && x.AuditoriumId == item.AuditoriumId);
            if (existNumberInAudi != null) throw new MovieTicketBookingExceptions ("Auditorium has number row  exist");
          
            var rowUpdate = item with
            {
                Id = Guid.Parse(id),
                Number = item.Number,
                AuditoriumId = item.AuditoriumId
            };
            _dbSet.Update(rowUpdate);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem is null) throw new MovieTicketBookingExceptions("Id is not exist");
            _dbSet.Remove(existItem);
            return true;
        }

        public override async Task<IEnumerable<Row>> GetAllAsync()
        {
           var rows = await _dbContext.Rows.AsTracking().ToListAsync();
            foreach(var item in rows)
            {
               await _dbContext.Entry(item).Reference(x => x.Auditorium).LoadAsync();
            }
            return rows;
        }

        public override async Task<Row> GetByIdAsync(Guid id)
        {
           var item = await _dbContext.Rows.AsTracking()
                                           .Where(x => x.Id == id)
                                           .Include(x => x.Auditorium)
                                           .ThenInclude(x => x.Rows).FirstOrDefaultAsync();
            return item;
        }


    }
}
