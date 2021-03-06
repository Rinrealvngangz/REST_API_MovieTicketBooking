using Core.IRepository;
using Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Extension;
namespace Core.Repository
{
   public class RoleRepository : GenericRepository<Role> , IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleRepository(RoleManager<Role> roleManager,
                              AppDbContext appDbContext,
                               UserManager<User> userManager) :base(appDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async override Task<Role> AddAsync(Role item)
        {
            var existRole =  await _roleManager.FindByNameAsync(item.Name);
            if (existRole != null) throw new MovieTicketBookingExceptions("Name role is exist");
           
            var result = await _roleManager.CreateAsync(item);
          
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync(item.Id.ToString());
                return role;
            }
            throw new MovieTicketBookingExceptions(result.Errors.AsToDescription());            
        }

        public override async Task<bool> UpdateAsync(string id ,Role item)
        {
           var existRole =  await _roleManager.FindByIdAsync(id);
            if(existRole ==null) throw new MovieTicketBookingExceptions("Role is not exist");
            existRole.Name = item.Name;
            existRole.Description = item.Description;
            await _roleManager.UpdateAsync(existRole);
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
           
            var existUserInRole =  await _userManager.GetUsersInRoleAsync(role.Name);
             if(existUserInRole.Count >0) throw new MovieTicketBookingExceptions("User has this role, Cannot delete");
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

    
    }
}
