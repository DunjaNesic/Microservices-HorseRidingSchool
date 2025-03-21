using Services.SessionAPI.Domain.Contracts;
using Services.SessionAPI.Infrastructure;

namespace Services.SessionAPI.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SessionDbContext _context;
        private readonly Lazy<ISessionRepo> _sessionRepository;
        private readonly Lazy<ISessionAssignedRepo> _sessionAssignedRepository;
        private readonly Lazy<ISessionDetailsRepo> _sessionDetailsRepository;


        public UnitOfWork(SessionDbContext context)
        {
            _context = context;
            _sessionRepository = new Lazy<ISessionRepo>(() => new SessionRepo(_context));
            _sessionAssignedRepository = new Lazy<ISessionAssignedRepo>(() => new SessionAssignedRepo(_context));
            _sessionDetailsRepository = new Lazy<ISessionDetailsRepo>(() => new SessionDetailsRepo(_context));
        }

        public ISessionRepo SessionRepository => _sessionRepository.Value;

        public ISessionAssignedRepo SessionAssignedRepository => _sessionAssignedRepository.Value;  

        public ISessionDetailsRepo SessionDetailsRepository => _sessionDetailsRepository.Value;

        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}
