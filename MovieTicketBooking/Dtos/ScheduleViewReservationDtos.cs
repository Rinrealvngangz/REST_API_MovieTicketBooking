using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class ScheduleViewReservationDtos
    {
        public Guid Id { get; set; }

        public DateTime Start { get; init; }


        public DateTime End { get; init; }


        public Decimal Price { get; init; }

        public MovieDtos Movie { get; init; }

    }
}
