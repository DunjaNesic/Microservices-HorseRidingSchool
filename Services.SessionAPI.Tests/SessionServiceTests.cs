using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Services.SessionAPI.ApplicationLayer;
using Services.SessionAPI.Domain.Contracts;
using Services.SessionAPI.Domain.DTO;
using Services.SessionAPI.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.SessionAPI.Tests
{
    public class SessionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly SessionService _service;

        public SessionServiceTests()
        {
            _service = new SessionService(_mockUow.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task UpsertSessionAsync_ReturnsBadRequest_WhenSessionDTOIsNull()
        {
            var result = await _service.UpsertSessionAsync(null);
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().Be("Session data is null.");
        }

        [Fact]
        public async Task UpsertSessionAsync_ReturnsSuccess_WhenSessionIsUpserted()
        {
            var sessionDTO = new SessionDTO
            {
                SessionAssigned = new SessionAssignedDTO(),
                SessionDetails = new List<SessionDetailsDTO>()
            };

            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(It.IsAny<int>()))
                .ReturnsAsync((SessionAssigned)null);

            _mockMapper.Setup(m => m.Map<SessionAssigned>(It.IsAny<SessionAssignedDTO>()))
                .Returns(new SessionAssigned());

            _mockMapper.Setup(m => m.Map<SessionPublishDTO>(sessionDTO))
                .Returns(new SessionPublishDTO());

            var result = await _service.UpsertSessionAsync(sessionDTO);

            result.IsSuccessful.Should().BeTrue();
            result.Message.Should().Be("Session upserted successfully.");
            result.Result.Should().BeOfType<SessionPublishDTO>();
        }

        [Fact]
        public async Task RemoveSessionAsync_ReturnsBadRequest_WhenSessionNotFound()
        {
            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(It.IsAny<int>()))
                .ReturnsAsync((SessionAssigned)null);

            var result = await _service.RemoveSessionAsync(1);

            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().Be("Session not found.");
        }

        [Fact]
        public async Task RemoveSessionAsync_ReturnsSuccess_WhenSessionRemoved()
        {
            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(It.IsAny<int>()))
                .ReturnsAsync(new SessionAssigned { SessionAssignedID = 1 });

            _mockUow.Setup(u => u.SessionDetailsRepository
                .GetSessionDetailsBySessionAssignedID(1))
                .ReturnsAsync(new List<SessionDetails>());

            var result = await _service.RemoveSessionAsync(1);

            result.IsSuccessful.Should().BeTrue();
            result.Message.Should().Be("Session removed successfully.");
        }

        [Fact]
        public async Task GetSessionAssigned_ReturnsDTO_WhenSessionExists()
        {
            var session = new SessionAssigned();
            var dto = new SessionAssignedDTO();

            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(1)).ReturnsAsync(session);
            _mockMapper.Setup(m => m.Map<SessionAssignedDTO>(session)).Returns(dto);

            var result = await _service.GetSessionAssigned(1);

            result.Should().BeSameAs(dto);
        }

        [Fact]
        public async Task GetSessionAssigned_ThrowsException_WhenSessionNotFound()
        {
            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(1)).ReturnsAsync((SessionAssigned)null);

            var act = async () => await _service.GetSessionAssigned(1);
            await act.Should().ThrowAsync<Exception>().WithMessage("Session not found.");
        }

        [Fact]
        public async Task GetSessionDetails_ReturnsDetails_WhenFound()
        {
            var assigned = new SessionAssigned { SessionAssignedID = 1 };
            var details = new List<SessionDetails> { new SessionDetails() };
            var detailsDTOs = new List<SessionDetailsDTO> { new SessionDetailsDTO() };

            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(1)).ReturnsAsync(assigned);
            _mockUow.Setup(u => u.SessionDetailsRepository.GetSessionDetailsBySessionAssignedID(1)).ReturnsAsync(details);
            _mockMapper.Setup(m => m.Map<IEnumerable<SessionDetailsDTO>>(details)).Returns(detailsDTOs);

            var result = await _service.GetSessionDetails(1);

            result.Should().BeEquivalentTo(detailsDTOs);
        }

        [Fact]
        public async Task GetSessionDetails_Throws_WhenAssignedNotFound()
        {
            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(1)).ReturnsAsync((SessionAssigned)null);

            var act = async () => await _service.GetSessionDetails(1);
            await act.Should().ThrowAsync<Exception>()
            .WithMessage("Error fetching session details: No session assigned found for session ID: 1");

        }

        [Fact]
        public async Task GetSessionDetails_Throws_WhenNoDetails()
        {
            var assigned = new SessionAssigned { SessionAssignedID = 1 };
            _mockUow.Setup(u => u.SessionAssignedRepository.GetSessionAssigned(1)).ReturnsAsync(assigned);
            _mockUow.Setup(u => u.SessionDetailsRepository.GetSessionDetailsBySessionAssignedID(1)).ReturnsAsync(new List<SessionDetails>());

            var act = async () => await _service.GetSessionDetails(1);
            await act.Should().ThrowAsync<Exception>()
            .WithMessage("Error fetching session details: No session details found for session assigned ID: 1");

        }
    }
}
