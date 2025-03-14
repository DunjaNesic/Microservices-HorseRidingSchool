using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthAPI.ApplicationLayer.IService;
using Services.AuthAPI.Domain.DTO;

namespace Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registrationDTO)
        {
            var errorMessage = await _authService.Register(registrationDTO);
            var response = new ResponseDTO();

            if (!string.IsNullOrEmpty(errorMessage)) {
                response.IsSuccessful = false;
                response.Message = errorMessage;
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = new ResponseDTO();

            var loginResponse = await _authService.Login(loginDTO);
            if (loginResponse.User == null) {
                response.IsSuccessful = false;
                response.Message = "Username or password is incorrect";
                return BadRequest(response);
            }

            response.Result = loginResponse;
            return Ok(response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationDTO registrationDTO)
        {
            var assignRoleSuccessful = await _authService.AssignRole(registrationDTO.Email, registrationDTO.Role);
            var response = new ResponseDTO();

            if (!assignRoleSuccessful)
            {
                response.IsSuccessful = false;
                response.Message = "Encountered an error";
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
