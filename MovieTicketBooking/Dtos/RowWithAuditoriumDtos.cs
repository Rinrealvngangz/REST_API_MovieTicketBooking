using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
   public class RowWithAuditoriumDtos : RowDtos
    {
      
        public AuditoriumDtos Auditorium { get; init; }
    }
}
