using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
namespace Core.IConfiguration
{
   public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IAuthenRepository Authen { get; }

        IRoleRepository Role { get; }

        IUserRoleRepository UserRole { get; }
        Task CompleteAsync();
    }
}
