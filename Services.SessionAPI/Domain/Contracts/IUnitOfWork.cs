namespace Services.SessionAPI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        ISessionRepo SessionRepository { get; }
        ISessionAssignedRepo SessionAssignedRepository { get; }
        ISessionDetailsRepo SessionDetailsRepository { get; }
        Task SaveChanges();
    }
}
