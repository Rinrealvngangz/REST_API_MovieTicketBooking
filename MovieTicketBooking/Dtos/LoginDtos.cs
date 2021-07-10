using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class LoginDtos
    {
        [Required(ErrorMessage = "Email  is required")]
        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        
    }
}
