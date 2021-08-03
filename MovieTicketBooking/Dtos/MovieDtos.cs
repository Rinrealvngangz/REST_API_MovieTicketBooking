using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dtos
{
    public  class MovieDtos
    {
      
        public string Id { get; init; }

        [Required]
        public string Name { get; init; }

        public string time { get; init; }

        [Required]
        public string Description { get; init; }


     
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode =true)]
        [Required]
        public string PublishedYear { get; init; }
     
    }

}
