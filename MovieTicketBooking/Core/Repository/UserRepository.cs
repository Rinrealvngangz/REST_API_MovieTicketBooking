using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Core.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async override Task<bool> AddAsync(User item)
        {
            var existUser = await _dbSet.Where(x => x.Email == item.Email).FirstOrDefaultAsync();
            if (existUser != null)
            {
                return await base.AddAsync(item);
            }
            throw new MovieTicketBookingExceptions("Exist email");
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

    }
}