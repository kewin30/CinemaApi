using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApi.Entities.Configurations
{
    public class ActorsConfiguration : IEntityTypeConfiguration<Actors>
    {
        public void Configure(EntityTypeBuilder<Actors> config)
        {
            config.Property(x => x.FullName).IsRequired().HasMaxLength(30);
            config.Property(x => x.Age).IsRequired().HasMaxLength(3);
            config.Property(x => x.Gender).IsRequired().HasMaxLength(6);
            config.Property(x => x.PlayingAs).IsRequired().HasMaxLength(30);

            config.HasOne(x => x.Films).WithMany(x=>x.Actors).HasForeignKey(x=>x.filmId).IsRequired(false);
                
        }
    }
}
