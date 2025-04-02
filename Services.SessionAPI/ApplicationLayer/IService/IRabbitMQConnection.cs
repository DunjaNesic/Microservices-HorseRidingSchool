using RabbitMQ.Client;

namespace Services.SessionAPI.ApplicationLayer.IService
{
    public interface IRabbitMQConnection
    {
        public IConnection Connection { get; }
        Task InitializeAsync();
    }
}
