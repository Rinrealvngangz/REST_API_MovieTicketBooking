using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
  public  class MovieExpand : MovieDtos
    {
        [Required]
        public int Hours { get; init; }

        [Required]
        public int Minutes { get; init; }

        [Required]
        public int Second { get; init; }
    }
}
