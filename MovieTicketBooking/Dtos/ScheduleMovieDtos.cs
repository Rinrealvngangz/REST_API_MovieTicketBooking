using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class ScheduleMovieDtos
    {
    
        [Required]
        public DateTime Start { get; init; }

        [Required]
        public Decimal Price { get; init; }

        [Required]
        public Guid AuthoriumId { get; init; }

        [Required]
        public Guid MovieId { get; init; }
    }
}
