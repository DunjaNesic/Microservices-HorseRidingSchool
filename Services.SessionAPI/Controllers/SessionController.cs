using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.SessionAPI.ApplicationLayer;
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.Controllers
{
    [Route("api/session")]
    [ApiController]
    //[Authorize]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly IHorseService _horseService;
        private readonly ITrainerService _trainerService;
        private readonly IMessageProducer _messageProducer;

        public SessionController(SessionService sessionService, IHorseService horseService, ITrainerService trainerService, IMessageProducer messageProducer)
        {
            _sessionService = sessionService;
            _horseService = horseService;
            _trainerService = trainerService;
            _messageProducer = messageProducer; 
        }

        [HttpPost]
        public async Task<IActionResult> SessionUpsert([FromBody] SessionDTO session)
        {
            if (session == null)
            {
                return BadRequest("Session is null");
            }

            var response = await _sessionService.UpsertSessionAsync(session);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Message);
            }

            var result = (SessionPublishDTO)response.Result;
            //_messageProducer.SendMessageAsync(result);
            

            return Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> SessionRemove([FromBody] int sessionAssignedID)
        {
            var response = await _sessionService.RemoveSessionAsync(sessionAssignedID);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Message);
            }

            var result = (SessionDetailsDTO)response.Result;

            return Ok(result);
        }

        [HttpGet("{sessionID}")]
        public async Task<ResponseDTO> GetSessions(int sessionID)
        {
            var response = new ResponseDTO();
            try
            {
                SessionAssignedDTO sessionAssigned = await _sessionService.GetSessionAssigned(sessionID);
                IEnumerable<SessionDetailsDTO> sessionDetails = await _sessionService.GetSessionDetails(sessionID);

                double total = 0;

                IEnumerable<HorseDTO> horses = await _horseService.GetAllHorses();
                TrainerDTO trainer = await _trainerService.GetTrainer(sessionAssigned.TrainerID);

                foreach (var detail in sessionDetails)
                {
                    detail.Horse = horses.FirstOrDefault(h => h.HorseID == detail.HorseID);

                    if (detail.IsOnPackage) total += 1500;
                    
                    else total += 1800;


                    total -= detail.Horse.HorsePrice;
                }

                total -= trainer.TrainerPrice;

                SessionDTO session = new SessionDTO() { 
                   SessionAssigned = sessionAssigned,
                   SessionDetails = sessionDetails,
                   Total = total        
                };

                response.IsSuccessful = true;
                response.Message = "Session fetched successfully."; 
                response.Result = session;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }
           await _messageProducer.SendMessageAsync(response.Result);

            return response;

        }

    }
}
