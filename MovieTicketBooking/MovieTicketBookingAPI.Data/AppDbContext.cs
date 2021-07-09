using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MovieTicketBookingAPI.Data.Entities;
using MovieTicketBookingAPI.Data.Configuration;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
namespace MovieTicketBookingAPI.Data
{
    public class AppDbContext : IdentityDbContext<User, Role,Guid>
    {
        public AppDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new AuditoriumConfig());
            builder.ApplyConfiguration(new MovieConfig());
            builder.ApplyConfiguration(new ReservationConfig());
            builder.ApplyConfiguration(new RowConfig());
            builder.ApplyConfiguration(new ScheduledMovieConfig());
            builder.ApplyConfiguration(new SeatConfig());
            builder.ApplyConfiguration(new SeatTypeConfig());
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
             base.OnModelCreating(builder);
        }

      
        public DbSet<Auditorium> Auditoriums { get; set; }
   
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Row> Rows { get; set; }

        public DbSet<ScheduledMovie> ScheduledMovies { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<SeatType> SeatTypes { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
