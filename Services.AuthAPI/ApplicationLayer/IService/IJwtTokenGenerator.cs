using Services.AuthAPI.Domain;

namespace Services.AuthAPI.ApplicationLayer.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
