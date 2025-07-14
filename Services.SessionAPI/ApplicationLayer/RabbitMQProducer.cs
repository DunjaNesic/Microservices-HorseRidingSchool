using RabbitMQ.Client;
using Services.SessionAPI.ApplicationLayer.IService;
using System.Text;
using System.Text.Json;

namespace Services.SessionAPI.ApplicationLayer
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IRabbitMQConnection _connection;

        public RabbitMQProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async Task SendMessageAsync<T>(T message)
        {

            //throw new Exception("Simulated failure: Notification service is down.");


            if (_connection.Connection == null)
            {
                Console.WriteLine("RabbitMQ connection is not initialized.");
                throw new InvalidOperationException("RabbitMQ connection is not initialized.");
            }

            Console.WriteLine("RabbitMQ connection established successfully.");

            using var channel = await _connection.Connection.CreateChannelAsync();

            Console.WriteLine("RabbitMQ channel created successfully.");

            await channel.QueueDeclareAsync(
                queue: "booking",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine("Queue 'booking' declared successfully.");

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));


            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "booking",
                body: body
            );

            Console.WriteLine("Message published to 'booking' successfully.");
        }
    }
}
