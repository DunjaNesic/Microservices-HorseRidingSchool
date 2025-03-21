namespace Services.SessionAPI.Domain.DTO
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
    }
}
