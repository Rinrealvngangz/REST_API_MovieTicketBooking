using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dtos
{
   public record AuditoriumDtos
    {

        public Guid Id { get; init; }

        [Required]
        public string Name { get; init; }
        [Required]
        public int Capacity { get; init; }
        
        public List<RowDtos> Rows { get; init; }




    }
}
