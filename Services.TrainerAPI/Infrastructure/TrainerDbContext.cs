using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Models;

namespace Services.TrainerAPI.Infrastructure
{
    public class TrainerDbContext : DbContext
    {
        public TrainerDbContext(DbContextOptions<TrainerDbContext> options) : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
    }
}
