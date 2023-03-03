using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApi.Entities.Configurations
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> config)
        {
            config.Property(x => x.Name).IsRequired().HasMaxLength(40);
            config.Property(x => x.City).IsRequired().HasMaxLength(40);
        }
    }
}
