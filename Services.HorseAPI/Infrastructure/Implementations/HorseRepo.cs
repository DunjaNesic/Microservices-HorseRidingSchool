using Microsoft.EntityFrameworkCore;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Domain.Contracts;
using Services.HorseAPI.Infrastructure;

namespace Services.HorseAPI.Infrastructure.Implementations
{
    public class HorseRepo : IHorseRepo
    {
        private readonly HorseDbContext _context;

        public HorseRepo(HorseDbContext context)
        {
            _context = context;
        }

        public IQueryable<Horse> GetAll()
        {
            return _context.Horses.AsQueryable();
        }
        public async Task<Horse?> Get(int horseID)
        {
            return await _context.Horses.FindAsync(horseID);
        }
        public async Task Create(Horse horse)
        {
            await _context.Horses.AddAsync(horse);
        }
  
    }
}
