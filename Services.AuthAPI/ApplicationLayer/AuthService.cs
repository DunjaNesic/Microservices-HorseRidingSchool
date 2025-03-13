using Microsoft.AspNetCore.Identity;
using Services.AuthAPI.ApplicationLayer.IService;
using Services.AuthAPI.Domain;
using Services.AuthAPI.Domain.Contracts;
using Services.AuthAPI.Domain.DTO;

namespace Services.AuthAPI.ApplicationLayer
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IUnitOfWork uow, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _uow = uow;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationDTO registrationDTO)
        {
            User user = new User() { 
                UserName = registrationDTO.Email,
                Email = registrationDTO.Email,
                NormalizedEmail = registrationDTO.Email.ToUpper(),
                Name = registrationDTO.Name,
                PhoneNumber = registrationDTO.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationDTO.Password);

                if (result.Succeeded)
                {

                    User? userToReturn = _uow.AuthRepository.GetUser(registrationDTO.Email);

                    UserDTO userDTO = new UserDTO()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };

                    return "";
                }
                else return result.Errors.FirstOrDefault().Description;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
