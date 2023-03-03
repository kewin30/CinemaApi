using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Entities
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options): base(options)
        {

        }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<CinemaSeats> CinemaSeats { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Films> Films { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
