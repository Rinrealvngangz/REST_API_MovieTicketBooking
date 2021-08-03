using System;
using System.Collections.Generic;

namespace MovieTicketBookingAPI.Data.Entities
{
    public record Movie
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public  TimeSpan Minutes { get; set; }

        public string Description { get; set; }

        public DateTime PublishedYear { get; set; }

        public ICollection<ScheduledMovie> ScheduledMovies { get; init; }
    }
}