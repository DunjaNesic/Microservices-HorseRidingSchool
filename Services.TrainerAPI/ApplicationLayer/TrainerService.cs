using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Domain.Contracts;
using Services.TrainerAPI.Infrastructure.Implementations;

namespace Services.TrainerAPI.ApplicationLayer
{
    public class TrainerService
    {
        private readonly IUnitOfWork _uow;

        public TrainerService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Trainer>> GetAllTrainersAsync()
        {
            return await _uow.TrainerRepository.GetAll().ToListAsync();
        }
        public async Task<Trainer?> GetTrainerByIdAsync(int trainerID)
        {
            return trainerID <= 0 ? null : await _uow.TrainerRepository.Get(trainerID);
        }
        public async Task CreateTrainerAsync(Trainer trainer)
        {
            if (trainer == null)
                throw new ArgumentNullException(nameof(trainer));

            await _uow.TrainerRepository.Create(trainer);
            await _uow.SaveChanges();
        }

    }
}
