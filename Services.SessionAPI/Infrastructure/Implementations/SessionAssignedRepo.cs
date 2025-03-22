using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;
using Services.SessionAPI.Domain.Contracts;
using System;

namespace Services.SessionAPI.Infrastructure.Implementations
{
    public class SessionAssignedRepo : ISessionAssignedRepo
    {
        private readonly SessionDbContext _context;

        public SessionAssignedRepo(SessionDbContext context)
        {
            _context = context;
        }

        public async Task<SessionAssigned?> GetSessionAssigned(int sessionID)
        {
            return await _context.SessionAssigned
                .FirstOrDefaultAsync(sa => sa.SessionAssignedID == sessionID);
        }

        public async Task Create(SessionAssigned sessionAssigned)
        {
            await _context.SessionAssigned.AddAsync(sessionAssigned);
        }

        public void Update(SessionAssigned sessionAssigned)
        {
            _context.SessionAssigned.Update(sessionAssigned);
        }
        public void Delete(SessionAssigned sessionAssigned)
        {
            _context.SessionAssigned.Remove(sessionAssigned);
        }

    }
}
