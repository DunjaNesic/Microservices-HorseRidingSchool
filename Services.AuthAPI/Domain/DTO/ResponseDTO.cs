﻿namespace Services.AuthAPI.Domain.DTO
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string? Message { get; set; } = "";
    }
}
