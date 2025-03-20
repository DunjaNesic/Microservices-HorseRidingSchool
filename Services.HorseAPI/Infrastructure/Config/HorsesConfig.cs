using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.HorseAPI.Domain;

namespace Services.HorseAPI.Infrastructure.Config
{
    public class HorsesConfig : IEntityTypeConfiguration<Horse>
    {
        public void Configure(EntityTypeBuilder<Horse> builder)
        {
            builder.HasData(
                new Horse
                {
                    HorseID = 1,
                    Name = "Waban",
                    Breed = "Thoroughbred",
                    DateOfBirth = new DateTime(2016, 4, 12),
                },
                new Horse
                {
                    HorseID = 2,
                    Name = "Silver Static",
                    Breed = "Arabian",
                    DateOfBirth = new DateTime(2016, 8, 15),
                },
                new Horse
                {
                    HorseID = 3,
                    Name = "Beba",
                    Breed = "Quarter Horse",
                    DateOfBirth = new DateTime(2014, 11, 10),
                },
                new Horse
                {
                    HorseID = 4,
                    Name = "Sena",
                    Breed = "Paint Horse",
                    DateOfBirth = new DateTime(2017, 2, 25),
                },
                new Horse
                {
                    HorseID = 5,
                    Name = "Crystal Night",
                    Breed = "Mustang",
                    DateOfBirth = new DateTime(2018, 6, 5),
                },
                new Horse
                {
                    HorseID = 6,
                    Name = "Princess",
                    Breed = "Pony",
                    DateOfBirth = new DateTime(2015, 7, 5),
                }
            );
        }
    }
}
