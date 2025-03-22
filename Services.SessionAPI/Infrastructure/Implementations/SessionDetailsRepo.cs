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

        public async Task<IEnumerable<SessionDetails>> GetSessionDetailsBySessionAssignedID(int sessionAssignedID)
        {
            return await _context.SessionDetails
                                 .Where(sd => sd.SessionAssignedID == sessionAssignedID)
                                 .ToListAsync();
        }

        public void Delete(SessionDetails sessionDetails)
        {
            _context.SessionDetails.Remove(sessionDetails);
        }

        public void DeleteRange(IEnumerable<SessionDetails> sessionDetailsList)
        {
            _context.SessionDetails.RemoveRange(sessionDetailsList);
        }


    }
}
