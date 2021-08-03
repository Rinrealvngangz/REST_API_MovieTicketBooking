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

        IAuditoriumRepository Auditorium { get; }

        IRowRepository Row { get; }

        ISeatRepository Seat { get; }

        ISeatTypeRepository SeatType { get; }

        IReservationRepository Reservation{ get; }

        IMovieRepository MovieRepository { get; }

        IScheduleMovieRepository ScheduleMovie { get; }
        Task CompleteAsync();
    }
}
