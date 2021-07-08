using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
    public record SeatType
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public ICollection<Seat> Seats { get; init; }

    }
}
