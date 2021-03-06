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
       
            await _dbSet.AddAsync(item);
            return item;
        }

        public override async Task<bool> UpdateAsync(string id, Movie item)
        {
           var existMovie = await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) );
        

            var existMovieName = await _dbSet.FirstOrDefaultAsync(x => x.Id != Guid.Parse(id) && x.Name == item.Name);
            if (existMovieName is not null) throw new MovieTicketBookingExceptions("Name movie is exist");

            existMovie.Name = item.Name;
            existMovie.Description = item.Description;
            existMovie.Minutes = item.Minutes;
            existMovie.PublishedYear = item.PublishedYear;
            
            _dbSet.Update(existMovie);
            return true;  
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var existMovie = await _dbContext.Movies.AsTracking().Include(x => x.ScheduledMovies).FirstOrDefaultAsync(x => x.Id == id);
            
 
            if (existMovie.ScheduledMovies.Count > 0) throw new MovieTicketBookingExceptions("Schedule movie is has exist");
             
            _dbSet.Remove(existMovie);
            return true;  
        }

        public override async Task<Movie> GetByIdAsync(Guid id)
        {
            var existMovie = await _dbContext.Movies.AsTracking()
                                                    .Include(x => x.ScheduledMovies).FirstOrDefaultAsync(x => x.Id == id);                                 
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