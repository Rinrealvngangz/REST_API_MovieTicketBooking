using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class SeatDtos
    {
        public Guid Id { get; set; }
        [Required]
        public Guid RowId { get; init; }

        public Guid SeatTypeId { get; init; }

        [Required]
        public int Number { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
