using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApi.Entities.Configurations
{
    public class CinemaSeatsConfiguration : IEntityTypeConfiguration<CinemaSeats>
    {
        public void Configure(EntityTypeBuilder<CinemaSeats> config)
        {
            config.Property(x => x.Name).IsRequired().HasMaxLength(40);
        }
    }
}
