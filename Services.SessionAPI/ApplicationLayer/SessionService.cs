using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.SessionAPI.Domain;
using Services.SessionAPI.Domain.Contracts;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer
{
    public class SessionService : ISessionService
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

                var sessionToPublish = _mapper.Map<SessionPublishDTO>(sessionDTO);
                sessionToPublish.Event = "Session_Published";

                response.IsSuccessful = true;
                response.Message = "Session upserted successfully.";
                response.Result = sessionToPublish;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error during session upsert: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseDTO> RemoveSessionAsync(int sessionAssignedID)
        {
            var response = new ResponseDTO();
            try
            {
                var existingSessionAssigned = await _uow.SessionAssignedRepository.GetSessionAssigned(sessionAssignedID);

                if (existingSessionAssigned == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "Session not found.";
                    return response;
                }

                var sessionDetailsList = await _uow.SessionDetailsRepository.GetSessionDetailsBySessionAssignedID(existingSessionAssigned.SessionAssignedID);

                if (sessionDetailsList.Any())
                {
                    _uow.SessionDetailsRepository.DeleteRange(sessionDetailsList);
                }

                _uow.SessionAssignedRepository.Delete(existingSessionAssigned);

                await _uow.SaveChanges();

                response.IsSuccessful = true;
                response.Message = "Session removed successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = $"Error during session removal: {ex.Message}";
            }

            return response;
        }

        public async Task<SessionAssignedDTO> GetSessionAssigned(int sessionAssignedID)
        {
            var sessionAssigned = await _uow.SessionAssignedRepository
                .GetSessionAssigned(sessionAssignedID);

            if (sessionAssigned == null)
            {
                throw new Exception("Session not found.");
            }

            var sessionAssignedDTO = _mapper.Map<SessionAssignedDTO>(sessionAssigned);
            return sessionAssignedDTO;
        }

        public async Task<IEnumerable<SessionDetailsDTO>> GetSessionDetails(int sessionID)
        {
            try
            {
                var sessionAssigned = await _uow.SessionAssignedRepository
                    .GetSessionAssigned(sessionID);

                if (sessionAssigned == null)
                {
                    throw new Exception($"No session assigned found for session ID: {sessionID}");
                }

                var sessionDetails = await _uow.SessionDetailsRepository
                    .GetSessionDetailsBySessionAssignedID(sessionAssigned.SessionAssignedID);

                if (sessionDetails == null || !sessionDetails.Any())
                {
                    throw new Exception($"No session details found for session assigned ID: {sessionAssigned.SessionAssignedID}");
                }

                var sessionDetailsDTOs = _mapper.Map<IEnumerable<SessionDetailsDTO>>(sessionDetails);
                return sessionDetailsDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching session details: {ex.Message}");
            }
        }
    }
}
