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
  public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository 
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public ReservationRepository(AppDbContext appDbContext, TokenValidationParameters tokenValidationParameters) :base(appDbContext)
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
                    throw new MovieTicketBookingExceptions ("token is false");
                }

            }
            var userId = tokenValidate.Claims.FirstOrDefault(x => x.Type == "ID").Value;

            if (userId is null) throw new MovieTicketBookingExceptions("userId is null");
            return userId;
        }
    }
}
