using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepository;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;

namespace Core.Repository
{
   public class SeatTypeRepository : GenericRepository<SeatType> , ISeatTypeRepository
    {
        public SeatTypeRepository(AppDbContext appContext):base(appContext)
        {

        }

    }
}
