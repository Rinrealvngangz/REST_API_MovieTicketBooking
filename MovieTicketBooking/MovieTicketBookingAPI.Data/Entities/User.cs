using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace MovieTicketBookingAPI.Data.Entities
{
   public class User : IdentityUser<Guid>
    {
      
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public bool? IsVip { get; init; }

        public ICollection<Reservation> Reservations { get; init; }
    }
}
