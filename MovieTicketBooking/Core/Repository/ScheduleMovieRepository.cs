using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data.Entities;
using MovieTicketBookingAPI.Data;
using Utilities.Exceptions;
using Core.IRepository;
namespace Core.Repository
{
    public class ScheduleMovieRepository : GenericRepository<ScheduledMovie>, IScheduleMovieRepository
    {
        private readonly IMovieRepository _movieRepository;
        public ScheduleMovieRepository(AppDbContext appDbContext, IMovieRepository movieRepository):base(appDbContext)
        {
            _movieRepository = movieRepository;
        }


        public override async Task<ScheduledMovie> AddAsync(ScheduledMovie item)
        {

            var existSchedule = await _dbSet.FirstOrDefaultAsync(x => x.Id == item.Id);
            if (existSchedule is not null) throw new MovieTicketBookingExceptions("exist schedule movie");
            var existTimeStart = await _dbSet.FirstOrDefaultAsync(x => x.Start == item.Start && x.MovieId == item.MovieId);
            if (existTimeStart is not null) throw new MovieTicketBookingExceptions("Start time schedule is exist");
            
            var movie =  await _movieRepository.GetByIdAsync(item.MovieId);
            if (movie == null) throw new MovieTicketBookingExceptions("Movie is not exist");

            var validTimeEnd = item.Start.AddHours(movie.Minutes.Hours)
                                         .AddMinutes(movie.Minutes.Minutes)
                                         .AddSeconds(movie.Minutes.Seconds);
            item.End = validTimeEnd;
            await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, ScheduledMovie item)
        {
            var existItem = await _dbSet.AsTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (existItem is null) throw new MovieTicketBookingExceptions("item is not exist");

            var existTimeStart = await _dbSet.FirstOrDefaultAsync(x => x.Start == item.Start
                                                                       && x.MovieId == item.MovieId && x.Id != Guid.Parse(id));
            if (existTimeStart is not null) throw new MovieTicketBookingExceptions("Start time schedule is exist");
          
            var movie = await _movieRepository.GetByIdAsync(item.MovieId);
            if (movie == null) throw new MovieTicketBookingExceptions("Movie is not exist");

          
            var validTimeEnd = item.Start.AddHours(movie.Minutes.Hours)
                                       .AddMinutes(movie.Minutes.Minutes)
                                       .AddSeconds(movie.Minutes.Seconds);
            existItem.Start = item.Start;
            existItem.End = validTimeEnd;
            existItem.Price = item.Price;
            existItem.MovieId = item.MovieId;
            existItem.AuthoriumId = item.AuthoriumId;
            _dbSet.Update(existItem);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
           var item = await _dbContext.ScheduledMovies.AsTracking().Include(x => x.Reservations)
                                            .FirstOrDefaultAsync( x => x.Id == id);
            if (item is null)
               throw new MovieTicketBookingExceptions("item is not exist");
            if (item.Reservations.Count > 0)
               throw new MovieTicketBookingExceptions("Cannot delete. Because have Reservations");
            _dbSet.Remove(item);
            return true;
        }
        public override async Task<ScheduledMovie> GetByIdAsync(Guid id)
        {
           var item =  await _dbContext.ScheduledMovies.AsTracking()
                              .Include(x => x.Auditorium).ThenInclude( a => a.Rows)
                              .ThenInclude( r => r.Seats).ThenInclude(t => t.SeatType)
                                                         .Include(x => x.Movie)
                                                         .Include(x => x.Reservations)
                                                         .FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) throw new MovieTicketBookingExceptions("item is not exist");
            return item;
        }
        public override async Task<IEnumerable<ScheduledMovie>> GetAllAsync()
        {
            var items = await _dbContext.ScheduledMovies.AsTracking()
                                .Include(x => x.Auditorium).ThenInclude(a => a.Rows)
                                .ThenInclude(r => r.Seats).ThenInclude(t => t.SeatType)
                                .Include(x => x.Movie)
                                .Include(x => x.Reservations)
                                .ToListAsync();
            if (items is null) throw new MovieTicketBookingExceptions("items is empty");
            return items;
        }
    }
   
}
