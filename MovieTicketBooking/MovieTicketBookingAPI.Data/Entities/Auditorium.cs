using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   public record Auditorium
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public int Capacity { get; init; }

        public ICollection<Row> Rows { get; init; }

        public ICollection<ScheduledMovie> ScheduledMovies { get; init; }


    }
}
