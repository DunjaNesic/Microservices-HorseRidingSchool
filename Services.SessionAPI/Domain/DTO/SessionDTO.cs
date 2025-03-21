namespace Services.SessionAPI.Domain.DTO
{
    public class SessionDTO
    {
        public SessionAssignedDTO SessionAssigned { get; set; }
        public List<SessionDetailsDTO> SessionDetails { get; set; }
        public double Total { get; set; }
    }
}
