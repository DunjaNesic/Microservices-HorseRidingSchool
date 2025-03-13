namespace Services.AuthAPI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IAuthRepo AuthRepository { get; }
        Task SaveChanges();
    }
}
