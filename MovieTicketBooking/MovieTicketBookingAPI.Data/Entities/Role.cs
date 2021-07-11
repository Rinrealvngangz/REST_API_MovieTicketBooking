using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace MovieTicketBookingAPI.Data.Entities
{
  public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
