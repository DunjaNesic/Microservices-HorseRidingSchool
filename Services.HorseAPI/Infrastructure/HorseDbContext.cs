using Microsoft.EntityFrameworkCore;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Infrastructure.Config;

namespace Services.HorseAPI.Infrastructure
{
    public class HorseDbContext : DbContext
    {
        public HorseDbContext(DbContextOptions<HorseDbContext> options) : base(options)
        {
        }
        public DbSet<Horse> Horses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HorsesConfig());
        }
    }
}
