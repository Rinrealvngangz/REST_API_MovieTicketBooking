using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   
    public record ScheduledMovie
    {
     
        public Guid Id { get; init; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public Decimal Price { get; set;}

        [System.Text.Json.Serialization.JsonIgnore]
        public Auditorium Auditorium { get; init; }

        public Guid AuthoriumId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Movie Movie { get; set; }

        public Guid MovieId { get; set; }

       [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Reservation> Reservations { get; init; }

    }
}
