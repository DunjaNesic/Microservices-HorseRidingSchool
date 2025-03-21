using System.ComponentModel.DataAnnotations;

namespace Services.SessionAPI.Domain
{
    public class SessionAssigned
    {
        [Key]
        public int SessionAssignedID { get; set; }
        public string TrainerName { get; set; }
        public DateTime Date { get; set; }

    }
}
