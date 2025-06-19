using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Services.TrainerAPI.ApplicationLayer;
using Services.TrainerAPI.Domain.Contracts;
using Services.TrainerAPI.Domain.DTO;
using Services.TrainerAPI.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.TrainerAPI.Tests
{
    public class TrainerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ITrainerRepo> _mockTrainerRepo;
        private readonly TrainerService _service;

        public TrainerServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockTrainerRepo = new Mock<ITrainerRepo>();

            _mockUow.Setup(u => u.TrainerRepository).Returns(_mockTrainerRepo.Object);

            _service = new TrainerService(_mockUow.Object, _mockMapper.Object);
        }

        //pogledati kasnije
        //[Fact]
        //public async Task GetAllTrainersAsync_ReturnsSuccessfulResponse()
        //{
        //    var trainers = new List<Trainer> { new Trainer { TrainerID = 1, Name = "Test" } };
        //    var trainerDTOs = new List<GetTrainerDTO> { new GetTrainerDTO { TrainerID = 1, Name = "Test" } };

        //    _mockTrainerRepo.Setup(r => r.GetAll()).Returns(trainers.AsQueryable());
        //    _mockMapper.Setup(m => m.Map<IEnumerable<GetTrainerDTO>>(trainers)).Returns(trainerDTOs);

        //    var result = await _service.GetAllTrainersAsync();

        //    result.IsSuccessful.Should().BeTrue();
        //    result.Result.Should().BeEquivalentTo(trainerDTOs);
        //}

        [Fact]
        public async Task GetTrainerByIdAsync_ReturnsError_WhenIdIsInvalid()
        {
            var result = await _service.GetTrainerByIdAsync(0);

            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().Be("Invalid Trainer ID.");
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ReturnsError_WhenTrainerNotFound()
        {
            _mockTrainerRepo.Setup(r => r.Get(1)).ReturnsAsync((Trainer)null);

            var result = await _service.GetTrainerByIdAsync(1);

            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().Be("Trainer not found.");
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ReturnsTrainer_WhenFound()
        {
            var trainer = new Trainer { TrainerID = 1, Name = "Ana" };
            var trainerDTO = new GetTrainerDTO { TrainerID = 1, Name = "Ana" };

            _mockTrainerRepo.Setup(r => r.Get(1)).ReturnsAsync(trainer);
            _mockMapper.Setup(m => m.Map<GetTrainerDTO>(trainer)).Returns(trainerDTO);

            var result = await _service.GetTrainerByIdAsync(1);

            result.IsSuccessful.Should().BeTrue();
            result.Result.Should().BeEquivalentTo(trainerDTO);
        }

        [Fact]
        public async Task CreateTrainerAsync_ReturnsError_WhenInputIsNull()
        {
            var result = await _service.CreateTrainerAsync(null);
          
            result.IsSuccessful.Should().BeFalse();
            result.Message.Should().Be("Trainer data is null.");
        }

        [Fact]
        public async Task CreateTrainerAsync_ReturnsSuccess_WhenValid()
        {
            var createDTO = new CreateTrainerDTO { Name = "Marko" };
            var trainer = new Trainer { TrainerID = 10, Name = "Marko" };

            _mockMapper.Setup(m => m.Map<Trainer>(createDTO)).Returns(trainer);
            _mockTrainerRepo.Setup(r => r.Create(trainer)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.SaveChanges()).Returns(Task.CompletedTask);

            var result = await _service.CreateTrainerAsync(createDTO);

            result.IsSuccessful.Should().BeTrue();
            result.Result.Should().BeEquivalentTo(trainer);
        }
    }
}
