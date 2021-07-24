using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class RowDtos
    {
        public Guid Id { get; init; }

        [Required]
        public int Number { get; init; }

        [Required]
        public Guid AuditoriumId { get; init; }

       
    }
}
