using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Domain.Contracts;
using Services.HorseAPI.Domain.DTO;

namespace Services.HorseAPI.ApplicationLayer
{
    public class HorseService : IHorseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public HorseService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> GetAllHorsesAsync()
        {
            var response = new ResponseDTO();
            try
            {
                var horses = await _uow.HorseRepository.GetAll().ToListAsync();
                var horsesDTOs = _mapper.Map<IEnumerable<GetHorseDTO>>(horses);

                response.Result = horsesDTOs;
                response.IsSuccessful = true;
                response.Message = "Horses retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error retrieving horses: {ex.Message}";
            }
            return response;
        }

        public async Task<ResponseDTO> GetHorseByIdAsync(int horseID)
        {
            var response = new ResponseDTO();
            try
            {
                if (horseID <= 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Invalid HorseID.";
                    return response;
                }

                var horse = await _uow.HorseRepository.Get(horseID);
                if (horse == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Horse not found.";
                    return response;
                }

                var horseDTO = _mapper.Map<GetHorseDTO>(horse);

                response.Result = horseDTO;
                response.IsSuccessful = true;
                response.Message = "Horse retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error retrieving horse: {ex.Message}";
            }
            return response;
        }

        public async Task<ResponseDTO> CreateHorseAsync(CreateHorseDTO createHorseDTO)
        {
            var response = new ResponseDTO();
            try
            {
                if (createHorseDTO == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Horse data is null.";
                    return response;
                }
                var trainer = _mapper.Map<Horse>(createHorseDTO);

                await _uow.HorseRepository.Create(trainer);
                await _uow.SaveChanges();

                response.Result = trainer;
                response.IsSuccessful = true;
                response.Message = "Horse created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error creating horse: {ex.Message}";
            }
            return response;
        }
    }
}
