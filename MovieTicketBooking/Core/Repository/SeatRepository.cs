using Core.IRepository;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data;
using Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Core.Repository
{
   public class SeatRepository : GenericRepository<Seat> , ISeatRepository
    {
        public SeatRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public override async Task<Seat> AddAsync(Seat item)
        {
            var exitsRow = await _dbContext.Rows.FindAsync(item.RowId);

            if (exitsRow == null) throw new MovieTicketBookingExceptions("Row is nor exist");

            var existSeat = _dbSet.FirstOrDefault(x => x.Number == item.Number && x.Name == item.Name && x.RowId == item.RowId);
           
            if (existSeat != null) throw new MovieTicketBookingExceptions("Exist Seat");
           

            await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, Seat item)
        {
            var exitsSeat = await _dbSet.FindAsync(Guid.Parse(id));
            if (exitsSeat == null) throw new MovieTicketBookingExceptions("Seat not exist");

           var exitsRow =  await _dbContext.Rows.FindAsync(exitsSeat.RowId);
            if (exitsRow == null) throw new MovieTicketBookingExceptions("Row is nor exist");

            var checkSeat = _dbSet.FirstOrDefault(x => x.Number == item.Number && x.Name == item.Name && x.RowId == item.RowId);
            if (checkSeat != null) throw new MovieTicketBookingExceptions("Seat has existed");

            _dbSet.Update(item);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var exitsSeat = await _dbContext.Seats.Where(x => x.Id == id).Include(x => x.Row).FirstOrDefaultAsync();
            if (exitsSeat == null) throw new MovieTicketBookingExceptions("Seat not exist");
             if(exitsSeat.Row == null)
            {
                _dbSet.Remove(exitsSeat);
                return true;
            }
            throw new MovieTicketBookingExceptions("Cannot delete because row exist contain seat");
            
        }

    }
}
