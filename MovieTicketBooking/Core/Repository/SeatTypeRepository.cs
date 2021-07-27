using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;
namespace Core.Repository
{
   public class SeatTypeRepository : GenericRepository<SeatType> , ISeatTypeRepository
    {
        public SeatTypeRepository(AppDbContext appContext):base(appContext)
        {

        }

        public override async Task<SeatType> AddAsync(SeatType item)
        {
           var existType = await _dbSet.FirstOrDefaultAsync(x => x.Name == item.Name);
            if (existType is not null) throw new MovieTicketBookingExceptions($"Exist Type has id:{existType.Id}");
           await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, SeatType item)
        {
           var existType =  await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (existType == null) throw new MovieTicketBookingExceptions("Seat type is not exist");
            existType.Name = item.Name;     
              _dbSet.Update(existType);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existType = await _dbSet.AsTracking()
                                        .Where(x => x.Id == id)
                                        .Include(x => x.Seats).FirstOrDefaultAsync();
            if (existType == null) throw new MovieTicketBookingExceptions("Seat type is not exist");
            if (existType.Seats.Count() > 0) throw new MovieTicketBookingExceptions("Cannot delete.Because exist Seat");
            _dbSet.Remove(existType);
            return true;

        }
        public override async Task<SeatType> GetByIdAsync(Guid id)
        {
           var seatType = await _dbContext.SeatTypes.AsTracking()
                               .Where( x => x.Id == id).FirstOrDefaultAsync();
            await _dbContext.Entry(seatType).Collection(x => x.Seats).LoadAsync();
            return seatType;
        }
        public override async Task<IEnumerable<SeatType>> GetAllAsync()
        {
            List<SeatType> seatTypes = new List<SeatType>();
            var seatType = await _dbContext.SeatTypes.AsTracking().ToListAsync();
            foreach( var seat in seatType)
            {
                await _dbContext.Entry(seat).Collection(x => x.Seats).LoadAsync();
                seatTypes.Add(seat);
            }
      
            return seatTypes;
        }
     
    }
}
