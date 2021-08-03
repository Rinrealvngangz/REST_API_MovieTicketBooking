using Dtos;
using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Utilities.Extension
{
    public static class GenericExtension
    {
        public static UserDtos AsToUserDtos(this User user)
        {
            return new UserDtos
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsVip = user.IsVip
            };
        }
        public async static Task<List<UserDtos>> AsToListUserDtos(this Task<IEnumerable<User>> users)
        {
            List<UserDtos> userDtos = new List<UserDtos>();
            var listUsers = await users;
            listUsers.ToList().ForEach(x => userDtos.Add(x.AsToUserDtos()));
            return userDtos;

        }

        public static RoleDtos AsToRoleDtos(this Role role)
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
            List<RowDtos> listRowDtos = null;
            if (auditorium.Rows.Count > 0)
            {
                listRowDtos = new List<RowDtos>();
                auditorium.Rows.ToList().ForEach(x => listRowDtos.Add(x.AsToRowDtos()));
            }


            return new AuditoriumDtos
            {
                Id = auditorium.Id,
                Name = auditorium.Name,
                Capacity = auditorium.Capacity,
                Rows = listRowDtos
            };
        }

        public static RowWithAuditoriumDtos AsToRowWithAuditoriumDtos(this Row row)
        {

            return new RowWithAuditoriumDtos
            {
                Id = row.Id,
                Number = row.Number,
                AuditoriumId = row.AuditoriumId,
                Auditorium = row.Auditorium.AsToAuditoriumDtos()
            };
        }

        public static RowDtos AsToRowDtos(this Row row)
        {

            return new RowDtos
            {
                Id = row.Id,
                Number = row.Number,
                AuditoriumId = row.AuditoriumId

            };
        }

        public static IEnumerable<AuditoriumDtos> AsToAuditoriumViewDtos(this IEnumerable<Auditorium> ListAuditorium)
        {

            var viewListAuditoriumDtos = new List<AuditoriumDtos>();
            foreach (var item in ListAuditorium)
            {
                var rows = new List<RowDtos>();
                if (item.Rows.Count > 0)
                {
                    item.Rows.ToList().ForEach(x => rows.Add(x.AsToRowDtos()));
                }
                rows.Sort((a, b) => a.Number - b.Number);
                var auditorium = new AuditoriumDtos
                {
                    Id = item.Id,
                    Name = item.Name,
                    Capacity = item.Capacity,
                    Rows = rows
                };
                viewListAuditoriumDtos.Add(auditorium);
            }
            return viewListAuditoriumDtos;


        }

        public static IEnumerable<RowWithAuditoriumDtos> AsToRowsDtosList(this IEnumerable<Row> rows)
        {
            var listRowDtos = new List<RowWithAuditoriumDtos>();
            foreach (var item in rows)
            {

                var row = new RowWithAuditoriumDtos
                {
                    Id = item.Id,
                    Number = item.Number,
                    AuditoriumId = item.AuditoriumId,
                    Auditorium = new AuditoriumDtos
                    {
                        Id = item.Auditorium.Id,
                        Name = item.Auditorium.Name,
                        Capacity = item.Auditorium.Capacity,

                    }
                };


                listRowDtos.Add(row);
            }
            listRowDtos.OrderBy(x => x.Id);

            return listRowDtos;
        }
        public static SeatDtos AsToSeatDtos(this Seat seat)
        {
            return new SeatDtos
            {
                Id = seat.Id,
                Name = seat.Name,
                Number = seat.Number,
                SeatTypeId = seat.SeatTypeId,
                RowId = seat.RowId,

            };
        }
        public static SeatTypeDtos AsToSeatTypeDtos(this SeatType seatTypes)
        {
            List<SeatDtos> seatDtos = new List<SeatDtos>();
            if(seatTypes.Seats.Count > 0)
            {
                foreach (var seat in seatTypes.Seats)
                {
                    seatDtos.Add(seat.AsToSeatDtos());
                }
            }
           

            return new SeatTypeDtos
            {
                Id = seatTypes.Id,
                Name = seatTypes.Name,
                Seats = seatDtos
            };
        }

        public static IEnumerable<SeatTypeDtos> AsToSeatTypeDtosList(this IEnumerable<SeatType> seatTypes)
        {
            List<SeatTypeDtos> seatTypeDtos = new List<SeatTypeDtos>();
            foreach (var seat in seatTypes)
            {
                seatTypeDtos.Add(seat.AsToSeatTypeDtos());
            }

            return seatTypeDtos;
        }

        public static MovieDtos AsToMovieDtos (this Movie movie)
        {
            var timeMovie = $"{movie.Minutes.Hours}:{movie.Minutes.Minutes}:{movie.Minutes.Seconds}";
            return new MovieDtos
            {
                Id = movie.Id.ToString(),
                Name = movie.Name,
                time = timeMovie,
                Description = movie.Description,
                PublishedYear = movie.PublishedYear.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
        }

        public static IEnumerable<MovieDtos> AsToMovieDtosList(this IEnumerable<Movie> movie)
        {
            List<MovieDtos> movieDtosList = new List<MovieDtos>();
            foreach (var item in movie)
            {
                var timeMovie = $"{item.Minutes.Hours}:{item.Minutes.Minutes}:{item.Minutes.Seconds}";
                movieDtosList.Add(new MovieDtos
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    time = timeMovie,
                    Description = item.Description,
                    PublishedYear = item.PublishedYear.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                });
            }
            return movieDtosList;
        }
    }
}
