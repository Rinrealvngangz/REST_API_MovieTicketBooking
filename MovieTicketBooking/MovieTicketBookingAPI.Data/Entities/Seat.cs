using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   public record Seat
    {
        public Guid Id { get; init; }

        public Guid RowId { get; init; }

        public Guid SeatTypeId { get; init; }

        public int Number { get; init; }

        public string Name { get; init; }

        public Row Row { get; init; }

        public SeatType SeatType { get; init; }

        public ICollection<Reservation> Reservations { get; init; }

    }
}
