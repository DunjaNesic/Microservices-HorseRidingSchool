namespace Services.AuthAPI.Domain.Contracts
{
    public interface IAuthRepo
    {
        User? GetUser(string email);

    }
}
