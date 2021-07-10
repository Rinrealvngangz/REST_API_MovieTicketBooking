using Dtos;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Extension
{
   public static class GenericExtension
    {
        public static UserDtos AsToUserDtos(this User user)
        {
            return new UserDtos
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsVip = user.IsVip
            };
        }
    }
}
