using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.TrainerAPI.Domain;

namespace Services.TrainerAPI.Infrastructure.Config
{
    public class TrainersConfig : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasData(
                new Trainer
                {
                    TrainerID = 1,
                    Name = "Sara Djokic",
                    Address = "Bulevar Kralja Aleksandra 50, Beograd",
                    PhoneNumber = "0641234567",
                    DateOfBirth = new DateTime(1985, 5, 20),
                    DateJoinedTheClub = new DateTime(2015, 8, 1)
                },
                new Trainer
                {
                    TrainerID = 2,
                    Name = "Sandra Kovacevic",
                    Address = "Njegoševa 15, Novi Sad",
                    PhoneNumber = "0659876543",
                    DateOfBirth = new DateTime(1990, 3, 15),
                    DateJoinedTheClub = new DateTime(2017, 6, 10)
                },
                new Trainer
                {
                    TrainerID = 3,
                    Name = "Vladimir Lazarevic",
                    Address = "Kralja Petra 88, Niš",
                    PhoneNumber = "063555888",
                    DateOfBirth = new DateTime(1982, 7, 5),
                    DateJoinedTheClub = new DateTime(2012, 9, 5)
                }
            );
        }
    }
}
