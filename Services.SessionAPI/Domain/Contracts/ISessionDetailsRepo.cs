namespace Services.SessionAPI.Domain.Contracts
{
    public interface ISessionDetailsRepo
    {
        Task<SessionDetails?> GetSessionDetails(int sessionDetailsID);
        Task Create(SessionDetails sessionDetails);
        void Update(SessionDetails sessionDetails);
    }
}
