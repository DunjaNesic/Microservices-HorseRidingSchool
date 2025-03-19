using Services.AuthAPI.Domain;

namespace Services.AuthAPI.ApplicationLayer.IService
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(User user);
    }
}
