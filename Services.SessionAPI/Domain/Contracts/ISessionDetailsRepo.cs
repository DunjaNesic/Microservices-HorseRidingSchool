namespace Services.SessionAPI.Domain.Contracts
{
    public interface ISessionDetailsRepo
    {
        Task<SessionDetails?> GetSessionDetails(int sessionDetailsID);
        Task Create(SessionDetails sessionDetails);
        void Update(SessionDetails sessionDetails);
        Task<IEnumerable<SessionDetails>> GetSessionDetailsBySessionAssignedID(int sessionAssignedID);
        void Delete(SessionDetails sessionDetails);
        void DeleteRange(IEnumerable<SessionDetails> sessionDetailsList);
    }
}
