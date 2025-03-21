using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;

namespace Services.SessionAPI.Infrastructure
{
    public class SessionDbContext : DbContext
    {
        public SessionDbContext(DbContextOptions<SessionDbContext> options) : base(options)
        {
        }
        public DbSet<SessionDetails> SessionDetails { get; set; }
        public DbSet<SessionAssigned> SessionAssigned { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
