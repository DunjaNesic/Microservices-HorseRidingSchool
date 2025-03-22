namespace Services.SessionAPI.Domain.DTO
{
    public class SessionAssignedDTO
    {
        public int SessionAssignedID { get; set; }
        public int TrainerID { get; set; }
        public TrainerDTO Trainer { get; set; }
        public DateTime Date { get; set; }
    }
}
