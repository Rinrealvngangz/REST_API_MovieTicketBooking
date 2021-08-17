using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class ReservationDtos
    {
        public Guid Id { get; init; }

        public bool? HasPaidTicket { get; init; }


        [Required]
        public Guid SeatId { get; init; }

        [Required]
        public Guid ScheduledMovieId { get; init; }

        [Required]
        public string Token { get; init; }

    }
}
