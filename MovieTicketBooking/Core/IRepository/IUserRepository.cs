using Dtos;
using Microsoft.AspNetCore.Identity;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task Login(User user);
        Task<VerifyEmailDtos> Register(UserDtos user);

        Task<UserDtos> VerifyEmail(VerifyEmailDtos verifyEmailDtos);
    }
}
