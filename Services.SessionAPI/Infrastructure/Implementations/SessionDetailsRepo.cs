using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;
using Services.SessionAPI.Domain.Contracts;
using System.Threading.Tasks;

namespace Services.SessionAPI.Infrastructure.Implementations
{
    public class SessionDetailsRepo : ISessionDetailsRepo
    {
        private readonly SessionDbContext _context;

        public SessionDetailsRepo(SessionDbContext context)
        {
            _context = context;
        }

        public async Task<SessionDetails?> GetSessionDetails(int sessionDetailsID)
        {
            return await _context.SessionDetails
                .FirstOrDefaultAsync(sd => sd.SessionDetailsID == sessionDetailsID);
        }

        public async Task Create(SessionDetails sessionDetails)
        {
            await _context.SessionDetails.AddAsync(sessionDetails);
        }

        public void Update(SessionDetails sessionDetails)
        {
            _context.SessionDetails.Update(sessionDetails);
        }
    }
}
