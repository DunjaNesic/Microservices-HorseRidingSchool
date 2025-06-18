using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.HorseAPI.Controllers;
using Services.HorseAPI.ApplicationLayer;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Domain.DTO;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.HorseAPI.Domain.Contracts;

namespace Services.HorseAPI.Tests
{
    public class HorseControllerTests
    {
        private readonly Mock<IHorseService> _mockHorseService;
        private readonly HorseController _controller;

        public HorseControllerTests()
        {
            _mockHorseService = new Mock<IHorseService>();
            _controller = new HorseController(_mockHorseService.Object);
        }

        [Fact]
        public async Task GetAllHorses_ReturnsOk_WhenSuccessful()
        {
            var horses = new List<GetHorseDTO>
            {
                new GetHorseDTO { HorseID = 1, Name = "Pegaz" },
                new GetHorseDTO { HorseID = 2, Name = "Tornado" }
            };

            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = horses,
                Message = "Success"
            };

            _mockHorseService.Setup(s => s.GetAllHorsesAsync()).ReturnsAsync(response);

            var result = await _controller.GetAllHorses();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDTO>(okResult.Value);
            Assert.True(returnValue.IsSuccessful);
            Assert.Equal(2, ((IEnumerable<GetHorseDTO>)returnValue.Result).AsList().Count);
        }

        [Fact]
        public async Task GetAllHorses_ReturnsNotFound_WhenFailed()
        {
            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Error retrieving horses."
            };

            _mockHorseService.Setup(s => s.GetAllHorsesAsync()).ReturnsAsync(response);

            var result = await _controller.GetAllHorses();

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error retrieving horses.", notFound.Value);
        }

        [Fact]
        public async Task GetHorseById_ReturnsOk_WhenFound()
        {
            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = new GetHorseDTO { HorseID = 1, Name = "Pegaz" },
                Message = "Success"
            };

            _mockHorseService.Setup(s => s.GetHorseByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetHorseById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDTO>(okResult.Value);
            Assert.True(returnValue.IsSuccessful);
            Assert.Equal("Pegaz", ((GetHorseDTO)returnValue.Result).Name);
        }

        [Fact]
        public async Task GetHorseById_ReturnsNotFound_WhenNotFound()
        {
            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Horse not found."
            };

            _mockHorseService.Setup(s => s.GetHorseByIdAsync(99)).ReturnsAsync(response);

            var result = await _controller.GetHorseById(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Horse not found.", notFound.Value);
        }

        [Fact]
        public async Task CreateHorse_ReturnsCreatedAt_WhenSuccessful()
        {
            var horseDto = new CreateHorseDTO { Name = "Nova" };
            var createdHorse = new Horse { HorseID = 5, Name = "Nova" };

            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = createdHorse,
                Message = "Created"
            };

            _mockHorseService.Setup(s => s.CreateHorseAsync(horseDto)).ReturnsAsync(response);

            var result = await _controller.CreateHorse(horseDto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ResponseDTO>(created.Value);
            Assert.True(returnValue.IsSuccessful);
            Assert.Equal("Nova", ((Horse)returnValue.Result).Name);
        }

        [Fact]
        public async Task CreateHorse_ReturnsBadRequest_WhenDtoIsNull()
        {
            var result = await _controller.CreateHorse(null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Horse is null", badRequest.Value);
        }

        [Fact]
        public async Task CreateHorse_ReturnsBadRequest_WhenServiceFails()
        {
            var horseDto = new CreateHorseDTO { Name = "Test" };

            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Creation failed"
            };

            _mockHorseService.Setup(s => s.CreateHorseAsync(horseDto)).ReturnsAsync(response);

            var result = await _controller.CreateHorse(horseDto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Creation failed", badRequest.Value);
        }
    }

    public static class EnumerableExtensions
    {
        public static List<T> AsList<T>(this IEnumerable<T> source) => new List<T>(source);
    }
}
