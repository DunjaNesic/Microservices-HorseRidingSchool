using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Services.SessionAPI.Controllers;
using Services.SessionAPI.ApplicationLayer;
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.DTO;
using Services.SessionAPI.Domain.Contracts;

namespace Services.SessionAPI.Tests
{
    public class SessionControllerTests
    {
        private readonly Mock<ISessionService> _mockSessionService = new();
        private readonly Mock<IHorseService> _mockHorseService = new();
        private readonly Mock<ITrainerService> _mockTrainerService = new();
        private readonly Mock<IMessageProducer> _mockMessageProducer = new();
        private readonly SessionController _controller;

        public SessionControllerTests()
        {
      _controller = new SessionController(
      _mockSessionService.Object,
      _mockHorseService.Object,
      _mockTrainerService.Object,
      _mockMessageProducer.Object
  );
        }

        [Fact]
        public async Task SessionUpsert_ReturnsBadRequest_WhenInputIsNull()
        {
            var result = await _controller.SessionUpsert(null);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SessionUpsert_ReturnsBadRequest_WhenServiceFails()
        {
            var dto = new SessionDTO();
            _mockSessionService.Setup(s => s.UpsertSessionAsync(dto)).ReturnsAsync(new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Something went wrong"
            });

            var result = await _controller.SessionUpsert(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SessionUpsert_ReturnsOk_WhenSuccessful()
        {
            var dto = new SessionDTO();
            var publishDTO = new SessionPublishDTO();
            _mockSessionService.Setup(s => s.UpsertSessionAsync(dto)).ReturnsAsync(new ResponseDTO
            {
                IsSuccessful = true,
                Result = publishDTO
            });

            var result = await _controller.SessionUpsert(dto);

            result.Should().BeOfType<OkObjectResult>();
            _mockMessageProducer.Verify(m => m.SendMessageAsync(publishDTO), Times.Once);
        }

        [Fact]
        public async Task SessionRemove_ReturnsBadRequest_WhenUnsuccessful()
        {
            _mockSessionService.Setup(s => s.RemoveSessionAsync(It.IsAny<int>())).ReturnsAsync(new ResponseDTO
            {
                IsSuccessful = false,
                Message = "Not found"
            });

            var result = await _controller.SessionRemove(10);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SessionRemove_ReturnsOk_WhenSuccessful()
        {
            var sessionDetails = new SessionDetailsDTO { HorseID = 1 };
            _mockSessionService.Setup(s => s.RemoveSessionAsync(1)).ReturnsAsync(new ResponseDTO
            {
                IsSuccessful = true,
                Result = sessionDetails
            });

            var result = await _controller.SessionRemove(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetSessions_ReturnsResponseDTO_WhenSuccessful()
        {
            var sessionID = 1;
            var sessionAssigned = new SessionAssignedDTO { TrainerID = 1 };
            var horse = new HorseDTO { HorseID = 1, HorsePrice = 500 };
            var trainer = new TrainerDTO { TrainerID = 1, TrainerPrice = 300 };
            var sessionDetails = new List<SessionDetailsDTO>
            {
                new SessionDetailsDTO { HorseID = 1, IsOnPackage = true }
            };

            _mockSessionService.Setup(s => s.GetSessionAssigned(sessionID)).ReturnsAsync(sessionAssigned);
            _mockSessionService.Setup(s => s.GetSessionDetails(sessionID)).ReturnsAsync(sessionDetails);
            _mockHorseService.Setup(s => s.GetAllHorses()).ReturnsAsync(new List<HorseDTO> { horse });
            _mockTrainerService.Setup(s => s.GetTrainer(1)).ReturnsAsync(trainer);

            var result = await _controller.GetSessions(sessionID);

            result.Should().BeOfType<ResponseDTO>();
            result.IsSuccessful.Should().BeTrue();
            result.Message.Should().Be("Session fetched successfully.");
            ((SessionDTO)result.Result).Total.Should().Be(1500 - 500 - 300);
            _mockMessageProducer.Verify(m => m.SendMessageAsync(It.IsAny<object>()), Times.Once);
        }
    }
}

