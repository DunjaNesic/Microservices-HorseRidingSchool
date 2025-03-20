namespace Services.HorseAPI.Domain.Contracts
{
    public interface IHorseRepo
    {
        IQueryable<Horse> GetAll();
        Task<Horse?> Get(int horseID);
        Task Create(Horse horse);

    }
}
