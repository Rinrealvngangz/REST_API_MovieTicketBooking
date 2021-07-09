using Core.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exceptions;
using NETCore.MailKit.Core;
using Dtos;
using Utilities.Extension;
namespace Core.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public UserRepository(AppDbContext appDbContext,
                              UserManager<User> userManager,
                              IEmailService emailService) : base(appDbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

     
        public async override Task<bool> UpdateAsync(User item)
        {
            var existuser = await GetByIdAsync(item.Id);
            if (existuser == null) throw new MovieTicketBookingExceptions("No exits user");

            var isExistMail = await _dbSet.Where(x => x.Email == item.Email).FirstOrDefaultAsync();

            if (isExistMail != null) throw new MovieTicketBookingExceptions("Already email");

            existuser.Email = item.Email;

            existuser.FirstName = item.FirstName;

            existuser.LastName = item.LastName;

            _dbSet.Update(existuser);

            return true;
        }

        public async override Task<bool> DeleteAsync(Guid id)
        {

            var existUser =   await GetByIdAsync(id);
            if (existUser == null) throw new MovieTicketBookingExceptions("No exits user");
            _dbSet.Remove(existUser);
            return true;
        }

        public Task Login(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<VerifyEmailDtos> Register(UserDtos user)
        {
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if(existUser != null) throw new MovieTicketBookingExceptions("Already email");
            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            var result =  await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                
               await _emailService.SendAsync(newUser.Email, "email verify", $"<p>Verify Email success this is code:{code}</p>", true);
                var isVerifyEmail = new VerifyEmailDtos
                {
                    UserId = newUser.Id,
                    Code = code
                };
                return isVerifyEmail;
            }
            else
            {
                throw new MovieTicketBookingExceptions($"{result.Errors.AsToDescription()}");
            }
            
            throw new MovieTicketBookingExceptions("Email is not exist");

        }

        public async Task<UserDtos> VerifyEmail(VerifyEmailDtos verifyEmailDtos)
        {
            
           var existUser = await _userManager.FindByIdAsync(verifyEmailDtos.UserId.ToString());
            if (existUser == null) throw new MovieTicketBookingExceptions("User is not exits");
             var result =  await _userManager.ConfirmEmailAsync(existUser, verifyEmailDtos.Code);
            if (result.Succeeded)
            {
             
                return new UserDtos()
                {
                    UserName =existUser.UserName,
                    FirstName =existUser.FirstName,
                    LastName =existUser.LastName,
                    Email =existUser.Email

                };

            }
            throw new MovieTicketBookingExceptions("Email is not exist");
        }
    }
}