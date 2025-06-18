using Microsoft.EntityFrameworkCore;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Infrastructure;
using Services.HorseAPI.Infrastructure.Implementations;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

public class HorseRepoTests
{
    private HorseDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<HorseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        var context = new HorseDbContext(options);

        context.Horses.AddRange(
      new Horse { HorseID = 1, Name = "Pegaz", Breed = "Lipicaner" },
      new Horse { HorseID = 2, Name = "Tornado", Breed = "Arapski" }
  );

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void GetAll_ReturnsAllHorses()
    {
        var context = CreateInMemoryDbContext();
        var repo = new HorseRepo(context);

        var result = repo.GetAll().ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, h => h.Name == "Pegaz");
    }

    [Fact]
    public async Task Get_ReturnsCorrectHorse_WhenExists()
    {
        var context = CreateInMemoryDbContext();
        var repo = new HorseRepo(context);

        var result = await repo.Get(1);

        Assert.NotNull(result);
        Assert.Equal("Pegaz", result!.Name);
    }

    [Fact]
    public async Task Get_ReturnsNull_WhenNotExists()
    {
        var context = CreateInMemoryDbContext();
        var repo = new HorseRepo(context);

        var result = await repo.Get(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_AddsNewHorse()
    {
        var options = new DbContextOptionsBuilder<HorseDbContext>()
            .UseInMemoryDatabase(databaseName: "CreateTest")
            .Options;

        using var context = new HorseDbContext(options);
        var repo = new HorseRepo(context);

        var newHorse = new Horse { HorseID = 3, Name = "Nova", Breed = "Lipicaner" };

        await repo.Create(newHorse);
        await context.SaveChangesAsync(); 

        var horses = context.Horses.ToList();
        Assert.Single(horses);
        Assert.Equal("Nova", horses[0].Name);
    }
}
