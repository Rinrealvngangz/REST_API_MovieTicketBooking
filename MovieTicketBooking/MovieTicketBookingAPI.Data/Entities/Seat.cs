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

        public Guid RowId { get; set; }

        public Guid SeatTypeId { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public Row Row { get; set; }

        public SeatType SeatType { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

    }
}
