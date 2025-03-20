using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Infrastructure.Config;
using Services.TrainerAPI.Domain;

namespace Services.TrainerAPI.Infrastructure
{
    public class TrainerDbContext : DbContext
    {
        public TrainerDbContext(DbContextOptions<TrainerDbContext> options) : base(options)
        {
        }
        public DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TrainersConfig());
        }
    }
}
