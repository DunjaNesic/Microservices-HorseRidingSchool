using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;
using Services.SessionAPI.Domain.Contracts;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer
{
    public class SessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> UpsertSessionAsync(SessionDTO sessionDTO)
        {
            var response = new ResponseDTO();
            try
            {
                if (sessionDTO == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Session data is null.";
                    return response;
                }

                var existingSessionAssigned = await _uow.SessionAssignedRepository.GetSessionAssigned(sessionDTO.SessionAssigned.SessionAssignedID);

                if (existingSessionAssigned != null)
                {
                    _mapper.Map(sessionDTO.SessionAssigned, existingSessionAssigned);
                    _uow.SessionAssignedRepository.Update(existingSessionAssigned);
                }
                else
                {
                    var newSessionAssigned = _mapper.Map<SessionAssigned>(sessionDTO.SessionAssigned);
                    await _uow.SessionAssignedRepository.Create(newSessionAssigned);
                    existingSessionAssigned = newSessionAssigned;  
                }

                await _uow.SaveChanges();  

                foreach (var detailsDTO in sessionDTO.SessionDetails)
                {
                    var existingSessionDetails = await _uow.SessionDetailsRepository
                        .GetSessionDetails(detailsDTO.SessionDetailsID);

                    if (existingSessionDetails != null)
                    {
                        _mapper.Map(detailsDTO, existingSessionDetails);
                        _uow.SessionDetailsRepository.Update(existingSessionDetails);
                    }
                    else
                    {
                        var newSessionDetails = _mapper.Map<SessionDetails>(detailsDTO);
                        newSessionDetails.SessionAssignedID = existingSessionAssigned.SessionAssignedID;  

                        await _uow.SessionDetailsRepository.Create(newSessionDetails);
                    }
                }

                await _uow.SaveChanges();

                response.IsSuccessful = true;
                response.Message = "Session upserted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error during session upsert: {ex.Message}";
            }

            return response;
        }



    }
}
