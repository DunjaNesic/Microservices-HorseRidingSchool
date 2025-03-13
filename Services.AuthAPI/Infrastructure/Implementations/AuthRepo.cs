using Services.AuthAPI.Domain;
using Services.AuthAPI.Domain.Contracts;

namespace Services.AuthAPI.Infrastructure.Implementations
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AuthDbContext _context;

        public AuthRepo(AuthDbContext context)
        {
            _context = context;
        }

        public User? GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == email);
        }
    }
}
