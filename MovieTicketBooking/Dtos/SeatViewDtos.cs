using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class SeatViewDtos : SeatDtos
    {
            
        public RowDtos Row { get; init; }

        public SeatTypeDtos  SeatType { get; init; }

    }
}
