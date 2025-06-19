using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Services.AuthAPI.ApplicationLayer;
using Services.AuthAPI.Domain.Contracts;
using Services.AuthAPI.Domain.DTO;
using Services.AuthAPI.Domain;
using System.Threading.Tasks;
using Services.AuthAPI.ApplicationLayer.IService;

namespace Services.AuthAPI.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null
            );

            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null
            );

            _authService = new AuthService(
                _mockUow.Object,
                _mockUserManager.Object,
                _mockJwtTokenGenerator.Object,
                _mockRoleManager.Object
            );
        }

        [Fact]
        public async Task Login_ReturnsNullUser_WhenUserDoesNotExist()
        {
            _mockUow.Setup(x => x.AuthRepository.GetUser("nonexistent")).Returns((User?)null);

            var result = await _authService.Login(new LoginDTO { UserName = "nonexistent", Password = "pass" });

            result.User.Should().BeNull();
            result.Token.Should().BeEmpty();
        }

        [Fact]
        public async Task Login_ReturnsNullUser_WhenPasswordIsInvalid()
        {
            var user = new User();
            _mockUow.Setup(x => x.AuthRepository.GetUser("user")).Returns(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, "wrong")).ReturnsAsync(false);

            var result = await _authService.Login(new LoginDTO { UserName = "user", Password = "wrong" });

            result.User.Should().BeNull();
            result.Token.Should().BeEmpty();
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenSuccessful()
        {
            var user = new User { Id = "1", Email = "user@example.com", Name = "User", PhoneNumber = "123" };

            _mockUow.Setup(x => x.AuthRepository.GetUser("user")).Returns(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, "correct")).ReturnsAsync(true);
            _mockJwtTokenGenerator.Setup(x => x.GenerateToken(user)).ReturnsAsync("valid_token");

            var result = await _authService.Login(new LoginDTO { UserName = "user", Password = "correct" });

            result.User.Should().NotBeNull();
            result.Token.Should().Be("valid_token");
        }

        [Fact]
        public async Task Register_ReturnsEmptyString_WhenSuccessful()
        {
            var dto = new RegistrationDTO
            {
                Email = "test@example.com",
                Password = "pass",
                Name = "Test",
                PhoneNumber = "123"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUow.Setup(x => x.AuthRepository.GetUser(dto.Email))
                .Returns(new User { Id = "1", Email = dto.Email, Name = dto.Name, PhoneNumber = dto.PhoneNumber });

            var result = await _authService.Register(dto);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_ReturnsErrorMessage_WhenCreationFails()
        {
            var identityError = new IdentityError { Description = "Invalid password" };
            var failedResult = IdentityResult.Failed(identityError);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), "badpass"))
                .ReturnsAsync(failedResult);

            var dto = new RegistrationDTO
            {
                Email = "bad@example.com",
                Password = "badpass",
                Name = "Bad",
                PhoneNumber = "000"
            };

            var result = await _authService.Register(dto);

            result.Should().Be("Invalid password");
        }

        [Fact]
        public async Task AssignRole_CreatesRoleAndAssigns_WhenNotExists()
        {
            var user = new User { Email = "user@example.com" };

            _mockUow.Setup(x => x.AuthRepository.GetUser(user.Email)).Returns(user);
            _mockRoleManager.Setup(x => x.RoleExistsAsync("ADMIN")).ReturnsAsync(false);
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(user, "ADMIN"))
            .ReturnsAsync(IdentityResult.Success);


            var result = await _authService.AssignRole(user.Email, "ADMIN");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task AssignRole_ReturnsFalse_WhenUserDoesNotExist()
        {
            _mockUow.Setup(x => x.AuthRepository.GetUser("missing@example.com")).Returns((User?)null);

            var result = await _authService.AssignRole("missing@example.com", "USER");

            result.Should().BeFalse();
        }
    }
}
