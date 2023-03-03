using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApi.Entities.Configurations
{
    public class FilmsConfiguration: IEntityTypeConfiguration<Films>
    {
        public void Configure(EntityTypeBuilder<Films> config)
        {
            config.Property(x => x.Name).IsRequired().HasMaxLength(35);
            config.Property(x => x.Description).IsRequired().HasMaxLength(100);
            config.Property(x => x.IsRecommended).IsRequired().HasMaxLength(2);
            config.Property(x => x.Genre).IsRequired().HasMaxLength(15);

        }
    }
}
