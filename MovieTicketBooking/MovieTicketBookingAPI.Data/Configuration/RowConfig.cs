using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    public class RowConfig : IEntityTypeConfiguration<Row>

    {
        public void Configure(EntityTypeBuilder<Row> builder)
        {
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.AuditoriumId).IsRequired();

        }
    }
}
