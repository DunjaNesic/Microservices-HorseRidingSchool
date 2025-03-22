using System.ComponentModel.DataAnnotations;

namespace Services.TrainerAPI.Domain.DTO
{
    public class GetTrainerDTO
    {
        public int TrainerID { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public double TrainerPrice { get; set; }
    }
}
