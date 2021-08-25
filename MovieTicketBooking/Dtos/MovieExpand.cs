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
        [Required(ErrorMessage ="Hours is a required")]
        public int Hours { get; init; }

        [Required(ErrorMessage = "Minutes is a required")]
        public int Minutes { get; init; }

        [Required(ErrorMessage = "Second is a required")]
        public int Second { get; init; }
    }
}
