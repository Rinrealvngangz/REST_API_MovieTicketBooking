﻿using Dtos;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Utilities.Extension
{
   public static class GenericExtension
    {
        public static UserDtos AsToUserDtos(this User user)
        {
            return new UserDtos
            {
                Id = user.Id,
                UserName =user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsVip = user.IsVip
            };
        }
        public async static Task<List<UserDtos>> AsToListUserDtos (this Task<IEnumerable<User>> users)
        {
              List<UserDtos> userDtos = new List<UserDtos>();
                   var listUsers =  await users;
              listUsers.ToList().ForEach(x => userDtos.Add(x.AsToUserDtos()));
            return userDtos;
               
        }

        public static RoleDtos AsToRoleDtos (this Role role)
        {
            return new RoleDtos
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }
    }
}
