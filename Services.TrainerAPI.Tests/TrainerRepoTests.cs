using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Infrastructure.Implementations;
using Services.TrainerAPI.Infrastructure;

namespace Services.TrainerAPI.Tests
{
    public class TrainerRepoTests
    {
        private TrainerDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TrainerDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new TrainerDbContext(options);
        }

        [Fact]
        public async Task Create_AddsTrainer_ToDatabase()
        {
            var context = CreateDbContext("Create_AddsTrainer");
            var repo = new TrainerRepo(context);

            var trainer = new Trainer { Name = "Marko" };

            await repo.Create(trainer);
            await context.SaveChangesAsync(); 

            var trainers = context.Trainers.ToList();
            trainers.Should().HaveCount(1);
            trainers[0].Name.Should().Be("Marko");
        }

        [Fact]
        public async Task Get_ReturnsTrainer_WhenExists()
        {
            var context = CreateDbContext("Get_ReturnsTrainer");
            var trainer = new Trainer { Name = "Ana" };
            context.Trainers.Add(trainer);
            await context.SaveChangesAsync();

            var repo = new TrainerRepo(context);

            var result = await repo.Get(trainer.TrainerID);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Ana");
        }

        [Fact]
        public async Task GetAll_ReturnsAllTrainers()
        {
            var context = CreateDbContext("GetAll_ReturnsAll");
            context.Trainers.AddRange(
                new Trainer { Name = "T1" },
                new Trainer { Name = "T2" }
            );
            await context.SaveChangesAsync();

            var repo = new TrainerRepo(context);

            var result = repo.GetAll().ToList();

            result.Should().HaveCount(2);
            result.Select(r => r.Name).Should().Contain(new[] { "T1", "T2" });
        }
    }
}
