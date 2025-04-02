namespace Services.SessionAPI.ApplicationLayer.IService
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message);
    }
}
