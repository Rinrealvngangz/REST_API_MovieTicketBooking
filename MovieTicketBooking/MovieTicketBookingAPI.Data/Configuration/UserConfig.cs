using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTicketBookingAPI.Data.Entities;

namespace MovieTicketBookingAPI.Data.Configuration
{
    class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUser");
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);
            builder.HasMany( x => x.Reservations)
                   .WithOne(x => x.User)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }

    }
    
}
