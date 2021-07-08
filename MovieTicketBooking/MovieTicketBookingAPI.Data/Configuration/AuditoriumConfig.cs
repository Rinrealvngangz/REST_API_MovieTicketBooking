using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    public class AuditoriumConfig : IEntityTypeConfiguration<Auditorium>
    {
        public void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Capacity).IsRequired();

            builder.HasMany(x => x.Rows).WithOne(x => x.Auditorium)
                   .HasForeignKey(x => x.AuditoriumId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ScheduledMovies).WithOne(x => x.Auditorium)
                   .HasForeignKey(x => x.AuthoriumId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}