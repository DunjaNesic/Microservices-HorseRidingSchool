using Services.HorseAPI.Domain.Contracts;
using Services.HorseAPI.Infrastructure;

namespace Services.HorseAPI.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HorseDbContext _context;
        private readonly Lazy<IHorseRepo> _horseRepository;

        public UnitOfWork(HorseDbContext context)
        {
            _context = context;
            _horseRepository = new Lazy<IHorseRepo>(() => new HorseRepo(_context));
        }
        public IHorseRepo HorseRepository => _horseRepository.Value;

        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}
