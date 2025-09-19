
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Services.SessionAPI.Infrastructure.Implementations
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public OutboxWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();

                    var messages = await uow.Context.OutboxMessages
                        .Where(m => !m.IsProcessed)
                        .OrderBy(m => m.OccurredOn)
                        .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            var payload = System.Text.Json.JsonSerializer.Deserialize<object>(message.Payload);
                            await messageProducer.SendMessageAsync(payload);

                            message.IsProcessed = true;
                            message.ProcessedOn = DateTime.UtcNow;
                        }
                        catch (Exception ex)
                        {
                            message.Error = ex.Message;
                        }
                    }

                    await uow.SaveChanges();
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OutboxWorker error: {ex.Message}");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}