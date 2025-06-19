using AutoMapper;
using Moq;
using Services.HorseAPI.ApplicationLayer;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Domain.Contracts;
using Services.HorseAPI.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class HorseServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IHorseRepo> _mockHorseRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly HorseService _service;

    public HorseServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHorseRepo = new Mock<IHorseRepo>();
        _mockMapper = new Mock<IMapper>();

        _mockUow.Setup(u => u.HorseRepository).Returns(_mockHorseRepo.Object);
        _service = new HorseService(_mockUow.Object, _mockMapper.Object);
    }

    //ovde nesto ne valja, pogledati kasnije
    //[Fact]
    //public async Task GetAllHorsesAsync_ReturnsSuccessfulResponse_WithMappedDtos()
    //{
    //    // Arrange
    //    var horses = new List<Horse>
    //    {
    //        new Horse { HorseID = 1, Name = "Pegaz" },
    //        new Horse { HorseID = 2, Name = "Tornado" }
    //    }.AsQueryable();

    //    _mockHorseRepo.Setup(r => r.GetAll()).Returns(horses);

    //    var horseDtos = new List<GetHorseDTO>
    //    {
    //        new GetHorseDTO { HorseID = 1, Name = "Pegaz" },
    //        new GetHorseDTO { HorseID = 2, Name = "Tornado" }
    //    };

    //    _mockMapper.Setup(m => m.Map<IEnumerable<GetHorseDTO>>(It.IsAny<IEnumerable<Horse>>()))
    //               .Returns(horseDtos);

    //    // Act
    //    var response = await _service.GetAllHorsesAsync();

    //    // Assert
    //    Assert.True(response.IsSuccessful);
    //    Assert.NotNull(response.Result);
    //    var resultList = Assert.IsAssignableFrom<IEnumerable<GetHorseDTO>>(response.Result);
    //    Assert.Equal(2, resultList.Count());
    //    Assert.Equal("Pegaz", resultList.First().Name);
    //}

    [Fact]
    public async Task GetAllHorsesAsync_ReturnsFailedResponse_WhenExceptionThrown()
    {
        _mockHorseRepo.Setup(r => r.GetAll()).Throws(new Exception("Database error"));

        var response = await _service.GetAllHorsesAsync();

        Assert.False(response.IsSuccessful);
        Assert.Contains("Database error", response.Message);
    }

    [Fact]
    public async Task GetHorseByIdAsync_ReturnsFailedResponse_WhenInvalidId()
    {
        var response = await _service.GetHorseByIdAsync(0);

        Assert.False(response.IsSuccessful);
        Assert.Equal("Invalid HorseID.", response.Message);
    }

    [Fact]
    public async Task GetHorseByIdAsync_ReturnsFailedResponse_WhenHorseNotFound()
    {
        _mockHorseRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((Horse?)null);

        var response = await _service.GetHorseByIdAsync(999);

        Assert.False(response.IsSuccessful);
        Assert.Equal("Horse not found.", response.Message);
    }

    [Fact]
    public async Task GetHorseByIdAsync_ReturnsSuccessfulResponse_WhenHorseFound()
    {
        var horse = new Horse { HorseID = 1, Name = "Pegaz" };
        var horseDto = new GetHorseDTO { HorseID = 1, Name = "Pegaz" };

        _mockHorseRepo.Setup(r => r.Get(1)).ReturnsAsync(horse);
        _mockMapper.Setup(m => m.Map<GetHorseDTO>(horse)).Returns(horseDto);

        var response = await _service.GetHorseByIdAsync(1);

        Assert.True(response.IsSuccessful);
        Assert.Equal(horseDto, response.Result);
    }

    [Fact]
    public async Task CreateHorseAsync_ReturnsFailedResponse_WhenDtoIsNull()
    {
        var response = await _service.CreateHorseAsync(null);

        Assert.False(response.IsSuccessful);
        Assert.Equal("Horse data is null.", response.Message);
    }

    [Fact]
    public async Task CreateHorseAsync_ReturnsSuccessfulResponse_WhenCreated()
    {
        var createDto = new CreateHorseDTO { Name = "Nova" };
        var horse = new Horse { HorseID = 5, Name = "Nova" };

        _mockMapper.Setup(m => m.Map<Horse>(createDto)).Returns(horse);
        _mockHorseRepo.Setup(r => r.Create(horse)).Returns(Task.CompletedTask);
        _mockUow.Setup(u => u.SaveChanges()).Returns(Task.CompletedTask);

        var response = await _service.CreateHorseAsync(createDto);

        Assert.True(response.IsSuccessful);
        Assert.Equal(horse, response.Result);
    }

    [Fact]
    public async Task CreateHorseAsync_ReturnsFailedResponse_WhenExceptionThrown()
    {
        var createDto = new CreateHorseDTO { Name = "Nova" };
        _mockMapper.Setup(m => m.Map<Horse>(createDto)).Returns(new Horse());

        _mockHorseRepo.Setup(r => r.Create(It.IsAny<Horse>())).ThrowsAsync(new Exception("DB error"));

        var response = await _service.CreateHorseAsync(createDto);

        Assert.False(response.IsSuccessful);
        Assert.Contains("DB error", response.Message);
    }
}
