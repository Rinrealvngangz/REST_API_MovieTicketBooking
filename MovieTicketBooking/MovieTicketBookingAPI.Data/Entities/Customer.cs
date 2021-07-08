using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MovieTicketBookingAPI.Data.Entities
{
    public record Customer
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public bool? IsVip { get; init; }

        public ICollection<Reservation> Reservations { get; init; }


    }
}