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

            //var props = new BasicProperties();
            //props.ContentType = "text/plain";
            //props.Expiration = "36000000";

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "booking",
                //true,
                //props,
                body: body
            );

            Console.WriteLine("Message published to 'booking' successfully.");
        }
    }
}
