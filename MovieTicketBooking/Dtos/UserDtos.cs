using System;
using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public record UserDtos
    {

        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool? IsVip { get; init; }
    }
}
