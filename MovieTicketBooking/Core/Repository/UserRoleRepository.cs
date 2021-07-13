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
    
        public async Task<bool> RemoveUserRoleAsync(string userId, List<string> IdRoles)
        {
           
            bool check = false;
            var  existUser = await _userManager.FindByIdAsync(userId);
                                 
            if (existUser is null) throw new MovieTicketBookingExceptions("User is not exits");

            foreach ( var item in IdRoles)
            {
                var existRole = await _roleManager.FindByNameAsync(item);
                if(existRole != null)
                {
                    check = true;
                  var usersInRole = await _userManager.GetUsersInRoleAsync(existRole.Name);
                    if (usersInRole  is not null)
                    {
                        var existUserInRole = usersInRole.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefault();
                        if (existUserInRole is not null)
                        {
                           await _userManager.RemoveFromRoleAsync(existUserInRole, existRole.Name);                        
                        }
                    }
                }
                throw new MovieTicketBookingExceptions("Role is not exits");
            }

            if (check == false) return false;
            return true;
        }


    }
}
