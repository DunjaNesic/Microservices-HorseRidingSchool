using System.ComponentModel.DataAnnotations;

namespace Services.TrainerAPI.Models.DTO
{
    public class GetTrainerDTO
    {
        public int TrainerID { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoinedTheClub { get; set; }
    }
}
