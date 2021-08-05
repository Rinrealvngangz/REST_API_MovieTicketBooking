using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class ScheduleViewDtos
    {
      
        public DateTime Start { get; init; }

       
        public DateTime End { get; init; }

   
        public Decimal Price { get; init; }

        public MovieDtos  Movie { get; init; }

        public AuditoriumDtos Auditorium  { get; init; }

        public ReservationDtos Reservation { get; init;}

    }
}
