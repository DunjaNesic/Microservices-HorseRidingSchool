namespace Services.TrainerAPI.Domain.Contracts
{
    public interface ITrainerRepo
    {
        IQueryable<Trainer> GetAll();
        Task<Trainer?> Get(int trainerID);
        Task Create(Trainer trainer);

    }
}
