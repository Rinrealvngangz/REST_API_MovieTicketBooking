using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    public class SeatTypeConfig : IEntityTypeConfiguration<SeatType>
    {
        public void Configure(EntityTypeBuilder<SeatType> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.HasMany(x => x.Seats).WithOne(x => x.SeatType).HasForeignKey(x => x.SeatTypeId);
        }
    }
}
