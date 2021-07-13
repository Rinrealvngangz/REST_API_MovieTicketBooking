
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRoleRepository
    {
        Task<bool> AddUserRoleAsync(string userId, string roleId);
        Task<bool> RemoveUserRoleAsync(string userId, List<string> roleId);

    }
}
