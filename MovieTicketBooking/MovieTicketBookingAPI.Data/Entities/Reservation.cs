using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   public record Reservation
    {
        public Guid Id { get; init; }

        public bool? HasPaidTicket { get; init; }

        public User User { get; init; }

        public Guid UserId { get; init; }

        public Seat Seat { get; init; }

        public Guid SeatId { get; init; }

        public ScheduledMovie ScheduledMovie { get; init; }

        public Guid ScheduledMovieId { get; init; }
    }
}
