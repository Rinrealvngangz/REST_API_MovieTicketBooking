using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingAPI.Data.Entities
{
   public record RefreshToken
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Token { get; init; }
        public string JwtId { get; init; }
        public bool IsUsed { get; init; }
        public bool IsRevorked { get; init; }
        public DateTime AddedDate { get; init; }
        public DateTime ExpiryDate { get; init; }

        [ForeignKey(nameof(UserId))]
        public  User user { get; set; }
    }
}
