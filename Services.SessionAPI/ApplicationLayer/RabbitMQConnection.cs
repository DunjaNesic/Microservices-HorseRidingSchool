using RabbitMQ.Client;
using Services.SessionAPI.ApplicationLayer.IService;
using System;
using System.Threading.Tasks;

namespace Services.SessionAPI.ApplicationLayer
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private IConnection? _connection;
        public IConnection Connection => _connection!;

        public async Task InitializeAsync()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    VirtualHost = "/",
                    Port = 5672

                };

                Console.WriteLine("Attempting to establish RabbitMQ connection...");

                _connection = await factory.CreateConnectionAsync();

                if (_connection.IsOpen)
                {
                    Console.WriteLine("RabbitMQ connection established successfully.");
                }
                else
                {
                    Console.WriteLine("RabbitMQ connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while establishing RabbitMQ connection: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                Console.WriteLine("Closing RabbitMQ connection...");
                _connection.Dispose();
                Console.WriteLine("RabbitMQ connection closed.");
            }
        }
    }
}
