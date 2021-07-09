using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;


namespace MovieTicketBookingAPI.Data.Configuration
{
    public class ReservationConfig : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.ScheduledMovieId).IsRequired();
            builder.Property(x => x.SeatId).IsRequired();
           
        }
    }
}
