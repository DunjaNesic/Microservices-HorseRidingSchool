namespace Services.HorseAPI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IHorseRepo HorseRepository { get; }
        Task SaveChanges();
    }
}
