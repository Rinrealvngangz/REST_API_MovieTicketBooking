using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace MovieTicketBookingAPI.Data.Entities
{
   public class User : IdentityUser<Guid>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

    }
}
