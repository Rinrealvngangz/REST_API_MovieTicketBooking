using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
using Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Repository
{
    public class ReservationRepository :  GenericRepository<Reservation>, IReservationRepository
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public ReservationRepository(AppDbContext appDbContext, TokenValidationParameters tokenValidationParameters) : base(appDbContext)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public override async Task<Reservation> AddAsync(Reservation item)
        {
            var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Id == item.Id);
            if (existItem != null) throw new MovieTicketBookingExceptions("exist item");



            var existSeat = await _dbContext.Seats.AsTracking().FirstOrDefaultAsync(x => x.Id == item.SeatId);
            if (existSeat == null) throw new MovieTicketBookingExceptions("seat is not exist");

            await _dbContext.Entry(existSeat).Collection(x => x.Reservations).LoadAsync();
            await _dbContext.Entry(existSeat).Reference(x => x.Row).LoadAsync();

            var scheduleMovie = await _dbContext.ScheduledMovies.FirstOrDefaultAsync(x => x.Id == item.ScheduledMovieId);

            if (scheduleMovie.AuthoriumId != existSeat.Row.AuditoriumId)
                throw new MovieTicketBookingExceptions("Seat is not exist in Autorium");


            if (existSeat.Reservations.Count >= 1)
                throw new MovieTicketBookingExceptions("Seats are already booked");

            var existScheduleMovie = await _dbContext.ScheduledMovies.FirstOrDefaultAsync(x => x.Id == item.ScheduledMovieId);
            if (existScheduleMovie == null) throw new MovieTicketBookingExceptions("Schedule movie not exist");

            await _dbSet.AddAsync(item);

            return item;

        }

        public string GetIdUserClaim(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidate = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);
                if (result == false)
                {
                    throw new MovieTicketBookingExceptions("Token is false");
                }

            }
            var userId = tokenValidate.Claims.FirstOrDefault(x => x.Type == "ID").Value;

            if (userId is null) throw new MovieTicketBookingExceptions("userId is null");
            return userId;
        }

        public override async Task<bool> UpdateAsync(string id, Reservation item)
        {
            var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (existItem == null) throw new MovieTicketBookingExceptions("not exist item");

           

            var existSeat = await _dbContext.Seats.AsTracking().FirstOrDefaultAsync(x => x.Id == item.SeatId);
            if (existSeat == null) throw new MovieTicketBookingExceptions("seat is not exist");

            await _dbContext.Entry(existSeat).Collection(x => x.Reservations).LoadAsync();
            await _dbContext.Entry(existSeat).Reference(x => x.Row).LoadAsync();

           

            var scheduleMovie = await _dbContext.ScheduledMovies.FirstOrDefaultAsync(x => x.Id == item.ScheduledMovieId);

            var existSeatByUser = await _dbSet.FirstOrDefaultAsync(x => x.ScheduledMovieId == scheduleMovie.Id && x.SeatId == item.SeatId);
            if (existSeatByUser != null) throw new MovieTicketBookingExceptions("You has booked this seat");
            if (scheduleMovie.AuthoriumId != existSeat.Row.AuditoriumId)
                throw new MovieTicketBookingExceptions("Seat is not exist in Autorium");


            if (existSeat.Reservations.Count >= 1 && existItem.UserId != item.UserId)
            {
               
               foreach(var res in existSeat.Reservations)
                {
                    await _dbContext.Entry(res).Reference(x => x.ScheduledMovie).LoadAsync();
                    if(res.ScheduledMovie.Start == scheduleMovie.Start)
                    {
                        throw new MovieTicketBookingExceptions("Seats are already booked");
                    }
                      
                }
              
            }

         

            existItem.SeatId = item.SeatId;
            existItem.ScheduledMovieId = item.ScheduledMovieId;

            _dbSet.Update(existItem);

            return true;
        }

        public async override Task<bool> DeleteAsync(Guid id)
        {
            var existItem = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem == null) throw new MovieTicketBookingExceptions("Not exist item");
             _dbSet.Remove(existItem);
            return true;
        }

        public async override Task<Reservation> GetByIdAsync(Guid id)
        {
            var existItem = await _dbSet.AsTracking().Include(x => x.ScheduledMovie).ThenInclude(x => x.Movie)
                                                     .Include(x => x.Seat).ThenInclude(x => x.SeatType)
                                                     .Include(x => x.User)
                                                     .FirstOrDefaultAsync(x => x.Id == id);
            await _dbContext.Entry(existItem.Seat).Reference(x => x.Row).LoadAsync();
            await _dbContext.Entry(existItem.Seat.Row).Reference(x => x.Auditorium).LoadAsync();

            if (existItem == null) throw new MovieTicketBookingExceptions("Not exist item");

            return existItem;
             
        }

        public override async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            var existItems = await _dbSet.AsTracking().Include(x => x.ScheduledMovie).ThenInclude(x => x.Movie)
                                                       .Include(x => x.Seat).ThenInclude(x => x.SeatType)
                                                       .Include(x => x.User)
                                                        .ToListAsync();
            foreach(var item in existItems)
            {
                await _dbContext.Entry(item.Seat).Reference(x => x.Row).LoadAsync();
                await _dbContext.Entry(item.Seat.Row).Reference(x => x.Auditorium).LoadAsync();
            }
           

            if (existItems == null) throw new MovieTicketBookingExceptions("Not exist item");

            return existItems;

        }
    }
}
