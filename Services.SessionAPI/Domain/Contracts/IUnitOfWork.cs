using Services.SessionAPI.Domain.DTO;
using Services.SessionAPI.Infrastructure;

namespace Services.SessionAPI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        ISessionRepo SessionRepository { get; }
        ISessionAssignedRepo SessionAssignedRepository { get; }
        ISessionDetailsRepo SessionDetailsRepository { get; }
        void AddOutboxMessage(string type, object payload);
        SessionDbContext Context { get; }
        Task SaveChanges();
    }
}
