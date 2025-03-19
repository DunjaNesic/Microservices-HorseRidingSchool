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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUnitOfWork uow, UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator, RoleManager<IdentityRole> roleManager)
        {
            _uow = uow;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            User? userToReturn = _uow.AuthRepository.GetUser(loginDTO.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(userToReturn, loginDTO.Password);
            if (userToReturn == null || !isValid) {
                return new LoginResponseDTO() { User = null, Token = "" };
            }

            var token = await _jwtTokenGenerator.GenerateToken(userToReturn);

            UserDTO userDTO = new UserDTO() {
                ID = userToReturn.Id,
                Email = userToReturn.Email,
                Name = userToReturn.Name,
                PhoneNumber = userToReturn.PhoneNumber           
            };

            LoginResponseDTO responseDTO = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token
            };

            return responseDTO;
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

        public async Task<bool> AssignRole(string email, string role)
        {
            User? userToReturn = _uow.AuthRepository.GetUser(email);

            if (userToReturn != null) {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }

                await _userManager.AddToRoleAsync(userToReturn, role);
                return true;
            }
            return false;
        }
    }
}
