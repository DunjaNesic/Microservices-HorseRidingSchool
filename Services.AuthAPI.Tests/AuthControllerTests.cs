using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Services.AuthAPI.Controllers;
using Services.AuthAPI.ApplicationLayer.IService;
using Services.AuthAPI.Domain.DTO;

namespace Services.AuthAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenServiceReturnsErrorMessage()
        {
            var registrationDTO = new RegistrationDTO();
            _mockAuthService.Setup(s => s.Register(registrationDTO))
                .ReturnsAsync("Email already exists");

            var result = await _controller.Register(registrationDTO);

            result.Should().BeOfType<BadRequestObjectResult>();
            var response = (result as BadRequestObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.IsSuccessful.Should().BeFalse();
            response.Message.Should().Be("Email already exists");
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            var registrationDTO = new RegistrationDTO();
            _mockAuthService.Setup(s => s.Register(registrationDTO))
                .ReturnsAsync((string?)null);

            var result = await _controller.Register(registrationDTO);

            result.Should().BeOfType<OkObjectResult>();
            var response = (result as OkObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenUserIsNull()
        {
            var loginDTO = new LoginDTO();
            _mockAuthService.Setup(s => s.Login(loginDTO))
                .ReturnsAsync(new LoginResponseDTO { User = null });

            var result = await _controller.Login(loginDTO);

            result.Should().BeOfType<BadRequestObjectResult>();
            var response = (result as BadRequestObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.IsSuccessful.Should().BeFalse();
            response.Message.Should().Be("Username or password is incorrect");
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginSuccessful()
        {
            var loginDTO = new LoginDTO();
            var loginResponse = new LoginResponseDTO { User = new UserDTO { Email = "user@example.com" } };

            _mockAuthService.Setup(s => s.Login(loginDTO))
                .ReturnsAsync(loginResponse);

            var result = await _controller.Login(loginDTO);

            result.Should().BeOfType<OkObjectResult>();
            var response = (result as OkObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.Result.Should().Be(loginResponse);
        }

        [Fact]
        public async Task AssignRole_ReturnsBadRequest_WhenServiceFails()
        {
            var registrationDTO = new RegistrationDTO { Email = "user@example.com", Role = "ADMIN" };

            _mockAuthService.Setup(s => s.AssignRole(registrationDTO.Email, registrationDTO.Role))
                .ReturnsAsync(false);

            var result = await _controller.AssignRole(registrationDTO);

            result.Should().BeOfType<BadRequestObjectResult>();
            var response = (result as BadRequestObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.IsSuccessful.Should().BeFalse();
            response.Message.Should().Be("Encountered an error");
        }

        [Fact]
        public async Task AssignRole_ReturnsOk_WhenSuccessful()
        {
            var registrationDTO = new RegistrationDTO { Email = "user@example.com", Role = "USER" };

            _mockAuthService.Setup(s => s.AssignRole(registrationDTO.Email, registrationDTO.Role))
                .ReturnsAsync(true);

            var result = await _controller.AssignRole(registrationDTO);

            result.Should().BeOfType<OkObjectResult>();
            var response = (result as OkObjectResult)?.Value as ResponseDTO;
            response.Should().NotBeNull();
            response!.IsSuccessful.Should().BeTrue();
        }
    }
}
