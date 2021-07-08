using System;
using System.Collections.Generic;

namespace MovieTicketBookingAPI.Data.Entities
{
    public record Movie
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public TimeSpan Minutes { get; init; }
        public string Description { get; init; }

        public DateTime PublishedYear { get; init; }

        public ICollection<ScheduledMovie> ScheduledMovies { get; init; }
    }
}