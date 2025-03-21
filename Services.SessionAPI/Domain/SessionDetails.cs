using Services.SessionAPI.Domain.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.SessionAPI.Domain
{
    public class SessionDetails
    {
        [Key]
        public int SessionDetailsID { get; set; }
        public int SessionAssignedID { get; set; }

        [ForeignKey("SessionAssignedID")]
        public SessionAssigned SessionAsigned { get; set; }
        public int HorseID { get; set; }

        [NotMapped]
        public HorseDTO Horse { get; set; }
        public bool IsOnPackage { get; set; }
        public string StudentName { get; set; }

    }
}
