using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer.IService
{
    public interface ITrainerService
    {
        Task<TrainerDTO> GetTrainer(int trainerID);
    }
}
