using System.ComponentModel.DataAnnotations;

namespace Services.TrainerAPI.Domain.DTO
{
    public class CreateTrainerDTO
    {
        public required string Name { get; set; }
        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoinedTheClub { get; set; } = DateTime.Now;

    }
}
