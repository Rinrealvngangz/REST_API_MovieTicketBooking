using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    public class SeatConfig : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.SeatTypeId).IsRequired();
            builder.HasMany(x => x.Reservations).WithOne(x => x.Seat).HasForeignKey(x => x.SeatId);
            builder.HasOne(x => x.Row).WithMany(x => x.Seats).HasForeignKey(x => x.RowId);

        }
    }
}
