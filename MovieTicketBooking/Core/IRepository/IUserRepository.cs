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
        Task<User> Login(LoginDtos login);
        Task<VerifyEmailDtos> Register(UserDtos user);

        Task<User> VerifyEmail(VerifyEmailDtos verifyEmailDtos);

        Task<bool> UpdateUserAsync(string id , string password ,User item);

        Task<IEnumerable<UserDtos>> GetAllUserRoleAsync();
        Task<VerifyEmailDtos> RegisterCustomer(UserDtos user);
    }
}
