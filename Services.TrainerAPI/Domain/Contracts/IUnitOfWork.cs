namespace Services.TrainerAPI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        ITrainerRepo TrainerRepository { get; }
        Task SaveChanges();
    }
}
