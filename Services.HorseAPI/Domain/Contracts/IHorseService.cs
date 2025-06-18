using Services.HorseAPI.Domain.DTO;

namespace Services.HorseAPI.Domain.Contracts
{
    public interface IHorseService
    {
        Task<ResponseDTO> GetAllHorsesAsync();
        Task<ResponseDTO> GetHorseByIdAsync(int horseId);
        Task<ResponseDTO> CreateHorseAsync(CreateHorseDTO dto);
    }
}
