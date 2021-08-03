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

    }
   
}
