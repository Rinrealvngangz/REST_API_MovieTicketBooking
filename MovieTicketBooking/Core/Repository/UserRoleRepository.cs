using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using Dtos;
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
        public async Task<bool> AddUserRoleAsync(string userId, List<RoleDtos> IdRoles)
        {
            var existUser = await  _userManager.FindByIdAsync(userId);
            List<string> NameRoles = new List<string>();
            if(existUser == null) throw new MovieTicketBookingExceptions("User is not exits");
            foreach (var item in IdRoles)
            {
                var existRole = await _roleManager.FindByIdAsync(item.Id.ToString());
                if (existRole == null) throw new MovieTicketBookingExceptions("Role is not exits");
                NameRoles.Add(item.Name);
            }
            if(NameRoles.Count > 0)
            {
                foreach (var role in NameRoles)
                {
                    await _userManager.AddToRoleAsync(existUser, role);
                }
            }
            else
            {
                return false;
            }

                return true;      
        }
    
        public async Task<bool> RemoveUserRoleAsync(string userId, List<RoleDtos> IdRoles)
        {
           
            bool check = false;
            var  existUser = await _userManager.FindByIdAsync(userId);
                                 
            if (existUser is null) throw new MovieTicketBookingExceptions("User is not exits");

            foreach ( var item in IdRoles)
            {
                var existRole = await _roleManager.FindByNameAsync(item.Name);
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
