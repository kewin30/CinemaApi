using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApi.Entities.Configurations
{
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> config)
        {
            config.Property(x => x.FullName).IsRequired().HasMaxLength(75);
            config.Property(x => x.Age).IsRequired().HasMaxLength(3);
            config.Property(x => x.Gender).IsRequired().HasMaxLength(6);
        }
    }
}
