using Services.NotificationAPI.ApplicationLayer;
using Services.NotificationAPI.ApplicationLayer.IService;

var builder = WebApplication.CreateBuilder(args);

// Registracija konekcije i konzumenta
builder.Services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
builder.Services.AddHostedService<RabbitMQConsumer>();

var app = builder.Build();

// Inicijalizacija konekcije na startu
using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
    await connection.InitializeAsync();
}

app.MapGet("/", () => "NotificationService running...");

app.Run();
