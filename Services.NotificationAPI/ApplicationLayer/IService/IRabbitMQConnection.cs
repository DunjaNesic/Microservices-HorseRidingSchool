using RabbitMQ.Client;

namespace Services.NotificationAPI.ApplicationLayer.IService
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }
        Task InitializeAsync();
    }
}
