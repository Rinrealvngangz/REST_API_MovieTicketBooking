using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class SeatTypeDtos
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public ICollection<SeatDtos> Seats { get; init; }

    }
}
