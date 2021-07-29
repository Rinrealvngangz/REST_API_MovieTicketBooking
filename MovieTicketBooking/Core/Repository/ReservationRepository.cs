using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieTicketBookingAPI.Data.Entities;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
namespace Core.Repository
{
  public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository 
    {
        public ReservationRepository(AppDbContext appDbContext):base(appDbContext)
        {

        }


    }
}
