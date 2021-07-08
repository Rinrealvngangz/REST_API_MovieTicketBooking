using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   public record Row
    {
        public Guid Id { get; init; }

        public int Number { get; init; }

        public ICollection<Seat> Seats { get; init; }

        public Guid AuditoriumId { get; init; }

        public Auditorium Auditorium { get; init; }


    }
}
