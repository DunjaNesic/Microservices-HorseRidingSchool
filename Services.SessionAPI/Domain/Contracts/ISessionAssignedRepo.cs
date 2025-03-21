namespace Services.SessionAPI.Domain.Contracts
{
    public interface ISessionAssignedRepo
    {
        Task<SessionAssigned?> GetSessionAssigned(int sessionID);
        Task Create(SessionAssigned sessionAssigned);
        void Update(SessionAssigned sessionAssigned);
    }
}
