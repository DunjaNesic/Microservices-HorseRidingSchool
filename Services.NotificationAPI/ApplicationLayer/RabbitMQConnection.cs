﻿using RabbitMQ.Client;
using Services.NotificationAPI.ApplicationLayer.IService;

namespace Services.NotificationAPI.ApplicationLayer
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
                    Port = 5672,
                };

                Console.WriteLine("Attempting to establish RabbitMQ connection...");

                _connection = await factory.CreateConnectionAsync();

                if (_connection.IsOpen)
                {
                    Console.WriteLine("RabbitMQ connection established.");
                }
                else
                {
                    Console.WriteLine("RabbitMQ connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ connection error: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                Console.WriteLine("Closing RabbitMQ connection...");
                _connection.Dispose();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}
