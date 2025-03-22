using Services.SessionAPI.Domain.DTO;
using System.ComponentModel.DataAnnotations;

namespace Services.SessionAPI.Domain
{
    public class SessionAssigned
    {
        [Key]
        public int SessionAssignedID { get; set; }
        public int TrainerID { get; set; }
        //public TrainerDTO Trainer { get; set; }
        public DateTime Date { get; set; }

    }
}
