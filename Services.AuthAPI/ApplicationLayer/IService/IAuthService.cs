using Services.AuthAPI.Domain.DTO;

namespace Services.AuthAPI.ApplicationLayer.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationDTO registrationDTO);
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);
    }
}
