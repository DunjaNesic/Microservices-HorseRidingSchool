using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;
using Services.SessionAPI.Domain.Contracts;

namespace Services.SessionAPI.Infrastructure.Implementations
{
    public class SessionRepo : ISessionRepo
    {
        private readonly SessionDbContext _context;

        public SessionRepo(SessionDbContext context)
        {
            _context = context;
        }
  
    }
}
