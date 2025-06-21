namespace Services.HorseAPI.Domain.DTO
{
    public class CreateHorseDTO
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int HorsePrice { get; set; }
    }
}
