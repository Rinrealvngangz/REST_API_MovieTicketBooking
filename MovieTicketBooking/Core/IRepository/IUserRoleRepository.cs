
using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRoleRepository
    {
        Task<bool> AddUserRoleAsync(string userId, List<RoleDtos> roleId);
        Task<bool> RemoveUserRoleAsync(string userId, List<RoleDtos> roleId);

    }
}
