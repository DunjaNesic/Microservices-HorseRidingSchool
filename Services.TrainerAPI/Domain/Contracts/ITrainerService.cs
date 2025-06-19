using Services.TrainerAPI.Domain.DTO;

namespace Services.TrainerAPI.Domain.Contracts
{
    public interface ITrainerService
    {
        public Task<ResponseDTO> GetAllTrainersAsync();
        public Task<ResponseDTO> GetTrainerByIdAsync(int trainerID);
        public Task<ResponseDTO> CreateTrainerAsync(CreateTrainerDTO createTrainerDTO);
    }
}
