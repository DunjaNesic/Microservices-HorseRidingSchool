using Services.SessionAPI.Domain.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.SessionAPI.Domain.DTO
{
    public class SessionDetailsDTO
    {
        public int SessionDetailsID { get; set; }
        public int SessionAssignedID { get; set; }
        //public SessionAssigned SessionAssigned { get; set; }
        public int HorseID { get; set; }
        public HorseDTO Horse { get; set; }
        public bool IsOnPackage { get; set; }
        public string StudentName { get; set; }


    }
}
