using System.ComponentModel.DataAnnotations;

namespace Services.SessionAPI.Domain.DTO
{
    public class TrainerDTO
    {
        public int TrainerID { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public double TrainerPrice { get; set; }
    }
}
