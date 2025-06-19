using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.Domain.Contracts
{
    public interface ISessionService
    {
        public Task<ResponseDTO> UpsertSessionAsync(SessionDTO sessionDTO);
        public Task<ResponseDTO> RemoveSessionAsync(int sessionAssignedID);
        public Task<SessionAssignedDTO> GetSessionAssigned(int sessionAssignedID);
        public Task<IEnumerable<SessionDetailsDTO>> GetSessionDetails(int sessionID);
    }
}
