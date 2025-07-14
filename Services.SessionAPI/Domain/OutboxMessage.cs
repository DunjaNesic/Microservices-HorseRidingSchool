namespace Services.SessionAPI.Domain
{
    public class OutboxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } 
        public string Payload { get; set; } 
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedOn { get; set; }
    }
}
