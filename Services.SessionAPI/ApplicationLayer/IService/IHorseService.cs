using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer.IService
{
    public interface IHorseService
    {
        Task<IEnumerable<HorseDTO>> GetAllHorses();
    }
}
