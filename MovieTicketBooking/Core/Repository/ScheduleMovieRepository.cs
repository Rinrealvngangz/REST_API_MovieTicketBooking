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
        public ScheduleMovieRepository(AppDbContext appDbContext):base(appDbContext)
        {

        }

        //public override async Task<ScheduledMovie> AddAsync(ScheduledMovie item)
        //{
        //   var existSchedule = await _dbSet.FirstOrDefaultAsync(x => x.Id == item.Id);        
        //     await _dbSet.AddAsync(item);
        //}


    }
   
}
