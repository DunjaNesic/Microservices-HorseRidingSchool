using RabbitMQ.Client.Events;
using Services.NotificationAPI.ApplicationLayer.IService;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Services.NotificationAPI.ApplicationLayer
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IRabbitMQConnection _connection;

        public RabbitMQConsumer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_connection.Connection == null)
            {
                Console.WriteLine("RabbitMQ connection is not initialized.");
                return;
            }

            var channel = await _connection.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "booking",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine("Listening to queue 'booking'...");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received message: {message}");

                //try
                //{
                //    var session = JsonSerializer.Deserialize<SessionDTO>(message);
                //    Console.WriteLine($"Session for TrainerID: {session?.SessionAssigned?.TrainerID}");
                //    Console.WriteLine($"Would send email to: {session?.SessionAssigned?.UserEmail}");
                //}
                //catch
                //{
                //    Console.WriteLine("Could not deserialize message.");
                //}

                await Task.Yield();
            };

             await channel.BasicConsumeAsync(
                queue: "booking",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
