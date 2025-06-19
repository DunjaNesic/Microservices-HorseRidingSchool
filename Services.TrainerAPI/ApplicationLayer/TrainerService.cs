using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Domain.Contracts;
using Services.TrainerAPI.Domain.DTO;
using Services.TrainerAPI.Infrastructure.Implementations;

namespace Services.TrainerAPI.ApplicationLayer
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> GetAllTrainersAsync()
        {
            var response = new ResponseDTO();
            try
            {
                var trainers = await _uow.TrainerRepository.GetAll().ToListAsync();
                var trainerDTOs = _mapper.Map<IEnumerable<GetTrainerDTO>>(trainers);

                response.Result = trainerDTOs;
                response.IsSuccessful = true;
                response.Message = "Trainers retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error retrieving trainers: {ex.Message}";
            }
            return response;
        }

        public async Task<ResponseDTO> GetTrainerByIdAsync(int trainerID)
        {
            var response = new ResponseDTO();
            try
            {
                if (trainerID <= 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Invalid Trainer ID.";
                    return response;
                }

                var trainer = await _uow.TrainerRepository.Get(trainerID);
                if (trainer == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Trainer not found.";
                    return response;
                }

                var trainerDTO = _mapper.Map<GetTrainerDTO>(trainer);

                response.Result = trainerDTO;
                response.IsSuccessful = true;
                response.Message = "Trainer retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error retrieving trainer: {ex.Message}";
            }
            return response;
        }

        public async Task<ResponseDTO> CreateTrainerAsync(CreateTrainerDTO createTrainerDTO)
        {
            var response = new ResponseDTO();
            try
            {
                if (createTrainerDTO == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Trainer data is null.";
                    return response;
                }
                var trainer = _mapper.Map<Trainer>(createTrainerDTO);

                await _uow.TrainerRepository.Create(trainer);
                await _uow.SaveChanges();

                response.Result = trainer;
                response.IsSuccessful = true;
                response.Message = "Trainer created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error creating trainer: {ex.Message}";
            }
            return response;
        }
    }
}
