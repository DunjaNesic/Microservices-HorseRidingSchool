using Services.AuthAPI.Domain.Contracts;
using Services.AuthAPI.Infrastructure;
using Services.AuthAPI.Infrastructure.Implementations;

namespace Services.TrainerAPI.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        private readonly Lazy<IAuthRepo> _authRepository;

        public UnitOfWork(AuthDbContext context)
        {
            _context = context;
            _authRepository = new Lazy<IAuthRepo>(() => new AuthRepo(_context));
        }

        public IAuthRepo AuthRepository => _authRepository.Value;

        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}
