using Dtos;
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

        public async static Task<List<RoleDtos>> AsToListRoleDtos(this Task<IEnumerable<Role>> roles)
        {
            List<RoleDtos> roleDtos = new List<RoleDtos>();
            var listUsers = await roles;
            listUsers.ToList().ForEach(x => roleDtos.Add(x.AsToRoleDtos()));
            return roleDtos;

        }

        public static AuditoriumDtos AsToAuditoriumDtos(this Auditorium auditorium)
        {

            return new AuditoriumDtos
            {
                Id = auditorium.Id,
                Name = auditorium.Name,
                Capacity = auditorium.Capacity
            };
        }

        public static RowDtos AsToRowDtos(this Row row)
        {

            return new RowDtos
            {
                Id = row.Id,
                Number = row.Number,
                AuditoriumId =row.AuditoriumId
            };
        }

        public static IEnumerable<AuditoriumDtos> AsToAuditoriumViewDtos(this IEnumerable<Auditorium> ListAuditorium)
        {
       
            var viewListAuditoriumDtos = new List<AuditoriumDtos>();
            foreach (var item in ListAuditorium)
            {
                var rows = new List<RowDtos>();
               if(item.Rows.Count >0 )
                {
                    item.Rows.ToList().ForEach(x => rows.Add(x.AsToRowDtos()));
                }
                  
                var auditorium = new AuditoriumDtos
                {
                    Id = item.Id,
                    Name = item.Name,
                    Rows = rows
                };
                viewListAuditoriumDtos.Add(auditorium);
            }
            return viewListAuditoriumDtos;


        }
    }
}
