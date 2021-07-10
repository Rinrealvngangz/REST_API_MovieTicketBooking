using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;
namespace Core.IRepository
{
    public interface IAuthenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshTokenDtos> GenerateToken(User user);

        Task<RefreshTokenDtos> VerifyRefreshToken(RefreshTokenDtos refreshToken);
    }
}
