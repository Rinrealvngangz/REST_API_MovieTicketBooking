using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class ReservationViewDtos
    {
        public Guid Id {  get; init; }

        public bool? HasPaidTicket { get; init; }

        public CustomerDtos Customer{ get; init; }

        public ScheduleViewReservationDtos ScheduleMovie { get;init;}

        public SeatViewDtos Seat { get; init; }

    }
}
