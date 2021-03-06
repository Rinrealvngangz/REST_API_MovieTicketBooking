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
                Auditorium = row.Auditorium.AsToAuditoriumDtos(),
                Seats = row.Seats.AsToSeatListDtos()
            };
        }

        public static RowDtos AsToRowDtos(this Row row)
        {

            return new RowDtos
            {
                Id = row.Id,
                Number = row.Number,
                AuditoriumId = row.AuditoriumId,
                Seats = row.Seats != null || row.Seats.Count > 0 ? row.Seats.AsToSeatListDtos() : null
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

                    },
                    Seats = item.Seats.AsToSeatListDtos()

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
        public static IEnumerable<SeatViewDtos> AsToSeatListDtos(this IEnumerable<Seat> seats)
        {
            List<SeatViewDtos> listSeat = new List<SeatViewDtos>();

            foreach (var item in seats)
            {
                listSeat.Add(item.AsToSeatViewDtos());
            }
            return listSeat;
        }



        public static SeatViewDtos AsToSeatViewDtos(this Seat seat)
        {
            return new SeatViewDtos
            {
                Id = seat.Id,
                Name = seat.Name,
                Number = seat.Number,
                //   Row = seat.Row != null ? seat.Row.AsToRowDtos() :null,
                SeatType = seat.SeatType != null ? seat.SeatType.AsToSeatTypeDtos() : null

            };
        }

        public static SeatTypeDtos AsToSeatTypeDtos(this SeatType seatTypes)
        {
            List<SeatDtos> seatDtos = new List<SeatDtos>();
            if (seatTypes.Seats.Count > 0 || seatTypes.Seats != null)
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

        public static MovieDtos AsToMovieDtos(this Movie movie)
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

        public static ScheduleViewDtos AsToScheduleMovieViews(this ScheduledMovie scheduledMovie)
        {
            return new ScheduleViewDtos
            {
                Id = scheduledMovie.Id,
                Start = scheduledMovie.Start,
                End = scheduledMovie.End,
                Price = scheduledMovie.Price,
                Movie = scheduledMovie.Movie.AsToMovieDtos(),
                Auditorium = scheduledMovie.Auditorium.AsToAuditoriumDtos()
            };
        }

        public static IEnumerable<ScheduleViewDtos> AsToScheduleMovieViewsList(this IEnumerable<ScheduledMovie> scheduledMovie)
        {
            List<ScheduleViewDtos> scheduledMovies = new List<ScheduleViewDtos>();
            foreach (var item in scheduledMovie)
            {
                scheduledMovies.Add(item.AsToScheduleMovieViews());
            }
            return scheduledMovies;
        }

        public static ReservationViewDtos AsToViewReservation(this Reservation reservation)
        {
            var reservationView = new ReservationViewDtos
            {
                Id = reservation.Id,
                HasPaidTicket = reservation.HasPaidTicket,
                Customer = new CustomerDtos
                {
                    Id = reservation.User.Id,
                    Email = reservation.User.Email,
                    FirstName = reservation.User.FirstName,
                    LastName = reservation.User.LastName,
                    IsVip = reservation.User.IsVip
                },
                Seat = new SeatViewDtos
                {
                    Id = reservation.Seat.Id,
                    Name = reservation.Seat.Name,
                    Number = reservation.Seat.Number,
                    RowId = reservation.Seat.RowId,
                    SeatTypeId = reservation.Seat.SeatTypeId,
                    SeatType = new SeatTypeDtos
                    {
                        Id = reservation.Seat.SeatType.Id,
                        Name = reservation.Seat.SeatType.Name
                    },
                    Row = new RowDtos
                    {
                        Id = reservation.Seat.Row.Id,
                        Number = reservation.Seat.Row.Number,
                        AuditoriumId = reservation.Seat.Row.AuditoriumId
                    }

                },
                ScheduleMovie = new ScheduleViewReservationDtos
                {
                    Id = reservation.ScheduledMovie.Id,
                    Start = reservation.ScheduledMovie.Start,
                    End = reservation.ScheduledMovie.End,
                    Price = reservation.ScheduledMovie.Price,
                    Movie = new MovieDtos
                    {
                        Id = reservation.ScheduledMovie.Movie.Id.ToString(),
                        Name = reservation.ScheduledMovie.Movie.Name,
                        Description = reservation.ScheduledMovie.Movie.Description,
                        time = reservation.ScheduledMovie.Movie.Minutes.ToString(),
                        PublishedYear = reservation.ScheduledMovie.Movie.PublishedYear.ToString()
                    }
                }

            };
            return reservationView;
        }

        public static IEnumerable<ReservationViewDtos> AsToViewReservationList(this IEnumerable<Reservation> listReservations)
        {
            var reservations = new List<ReservationViewDtos>();
            foreach (var item in listReservations)
            {
                reservations.Add(item.AsToViewReservation());
            }
            return reservations;
        }
    }
}
