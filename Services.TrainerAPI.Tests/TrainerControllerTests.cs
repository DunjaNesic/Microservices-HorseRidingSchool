using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Services.TrainerAPI.Controllers;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Domain.DTO;
using Services.TrainerAPI.Domain.Contracts;

namespace Services.TrainerAPI.Tests.Controllers
{
    public class TrainerControllerTests
    {
        private readonly Mock<ITrainerService> _mockService;
        private readonly TrainerController _controller;

        public TrainerControllerTests()
        {
            _mockService = new Mock<ITrainerService>();
            _controller = new TrainerController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllTrainers_ReturnsOk_WhenSuccessful()
        {
            var trainers = new List<Trainer> { new Trainer { TrainerID = 1, Name = "John" } };
            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = trainers
            };
            _mockService.Setup(s => s.GetAllTrainersAsync()).ReturnsAsync(response);

            var result = await _controller.GetAllTrainers();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetAllTrainers_ReturnsNotFound_WhenUnsuccessful()
        {
            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "No trainers found"
            };
            _mockService.Setup(s => s.GetAllTrainersAsync()).ReturnsAsync(response);

            var result = await _controller.GetAllTrainers();

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetTrainerById_ReturnsOk_WhenSuccessful()
        {
            var trainer = new Trainer { TrainerID = 1, Name = "Ana" };
            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = trainer
            };
            _mockService.Setup(s => s.GetTrainerByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetTrainerById(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetTrainerById_ReturnsNotFound_WhenUnsuccessful()
        {
            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Trainer not found"
            };
            _mockService.Setup(s => s.GetTrainerByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetTrainerById(1);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateTrainer_ReturnsBadRequest_WhenInputIsNull()
        {
            var result = await _controller.CreateTrainer(null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateTrainer_ReturnsBadRequest_WhenUnsuccessful()
        {
            var dto = new CreateTrainerDTO { Name = "Nina" };
            var response = new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Validation error"
            };
            _mockService.Setup(s => s.CreateTrainerAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CreateTrainer(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateTrainer_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var dto = new CreateTrainerDTO { Name = "Marko" };
            var createdTrainer = new Trainer { TrainerID = 99, Name = "Marko" };
            var response = new ResponseDTO
            {
                IsSuccessful = true,
                Result = createdTrainer
            };
            _mockService.Setup(s => s.CreateTrainerAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CreateTrainer(dto);

            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.ActionName.Should().Be(nameof(TrainerController.GetTrainerById));
            createdResult.RouteValues["id"].Should().Be(99);
            createdResult.Value.Should().BeEquivalentTo(response);
        }
    }
}
