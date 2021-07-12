using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using Microsoft.AspNetCore.Identity;
using MovieTicketBookingAPI.Data.Entities;
using Utilities.Exceptions;

namespace Core.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserRoleRepository(UserManager<User> userManager,
                                  RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AddUserRoleAsync(string userId, string roleId)
        {
             var existUser = await  _userManager.FindByIdAsync(userId);

             if(existUser == null) throw new MovieTicketBookingExceptions("User is not exits");

            var existRole = await _roleManager.FindByIdAsync(roleId);

            if(existRole == null) throw new MovieTicketBookingExceptions("Role is not exits");

             var result = await _userManager.AddToRoleAsync(existUser, existRole.Name);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
