using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Domain.Contracts;

namespace Services.TrainerAPI.Infrastructure.Implementations
{
    public class TrainerRepo : ITrainerRepo
    {
        private readonly TrainerDbContext _context;

        public TrainerRepo(TrainerDbContext context)
        {
            _context = context;
        }

        public IQueryable<Trainer> GetAll()
        {
            return _context.Trainers.AsQueryable();
        }
        public async Task<Trainer?> Get(int trainerID)
        {
            return await _context.Trainers.FindAsync(trainerID);
        }
        public async Task Create(Trainer trainer)
        {
            await _context.Trainers.AddAsync(trainer);
        }
  
    }
}
