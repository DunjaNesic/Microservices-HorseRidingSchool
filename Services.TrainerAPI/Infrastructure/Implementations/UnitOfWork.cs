using Services.TrainerAPI.Domain.Contracts;
using Services.TrainerAPI.Infrastructure;

namespace Services.TrainerAPI.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TrainerDbContext _context;
        private readonly Lazy<ITrainerRepo> _trainerRepository;

        public UnitOfWork(TrainerDbContext context)
        {
            _context = context;
            _trainerRepository = new Lazy<ITrainerRepo>(() => new TrainerRepo(_context));
        }

        public ITrainerRepo TrainerRepository => _trainerRepository.Value;

        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}
