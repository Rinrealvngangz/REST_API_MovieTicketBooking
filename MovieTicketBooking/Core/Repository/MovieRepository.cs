using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using MovieTicketBookingAPI.Data.Entities;
using MovieTicketBookingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;
namespace Core.Repository
{
   public class MovieRepository : GenericRepository<Movie> , IMovieRepository
    {

        public MovieRepository(AppDbContext appDbContext):base(appDbContext)
        {
            
        }

        public override async Task<Movie> AddAsync(Movie item)
        {
           var existMovie = await _dbSet.FirstOrDefaultAsync(x => x.Name == item.Name.ToLower());
            if (existMovie is not  null) throw new MovieTicketBookingExceptions("Exist movie!");
            await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, Movie item)
        {
           var existMovie = await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
           if (existMovie is null)  throw new MovieTicketBookingExceptions("Is not exist Movie");
            var isUpdate = item with
            {
                Name = item.Name,
                Description = item.Description,
                Minutes = item.Minutes,
                PublishedYear = item.PublishedYear
            };
             _dbSet.Update(isUpdate);
            return true;  
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existMovie = await _dbContext.Movies.AsTracking().Include(x => x.ScheduledMovies).FirstOrDefaultAsync(x => x.Id == id);
            
            if (existMovie is null) throw new MovieTicketBookingExceptions("Movie is not exist");

            if (existMovie.ScheduledMovies.Count > 0) throw new MovieTicketBookingExceptions("Schedule movie is has exist");
             
            _dbSet.Remove(existMovie);
            return true;  
        }

        public override async Task<Movie> GetByIdAsync(Guid id)
        {
            var existMovie = await _dbContext.Movies.AsTracking()
                                                    .Include(x => x.ScheduledMovies).FirstOrDefaultAsync();
            if (existMovie is null) throw new MovieTicketBookingExceptions("Movie is not exist");                                   
             return existMovie;
        }
        public override async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var existMovies = await _dbContext.Movies.AsTracking().ToListAsync();
            List<Movie> listMovies = new List<Movie>();
            foreach( var item in existMovies)
            {
                listMovies.Add(item);
            }
            return listMovies;
        }
    }
}