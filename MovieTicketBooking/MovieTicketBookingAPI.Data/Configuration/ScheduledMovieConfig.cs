using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    public class ScheduledMovieConfig : IEntityTypeConfiguration<ScheduledMovie>
    {
        public void Configure(EntityTypeBuilder<ScheduledMovie> builder)
        {
            builder.Property(x => x.Start).IsRequired();
            builder.Property(x => x.End).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.MovieId).IsRequired();
            builder.HasMany(x => x.Reservations).WithOne(x => x.ScheduledMovie).HasForeignKey(x => x.ScheduledMovieId);
            builder.HasOne(x => x.Movie).WithMany(x => x.ScheduledMovies).HasForeignKey(x => x.MovieId);
        }
    }
}
