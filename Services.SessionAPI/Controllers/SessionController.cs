using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.SessionAPI.ApplicationLayer;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
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

            var result = (SessionDetailsDTO)response.Result;

            //if (response.IsNewlyCreated)
            //{
            //    return CreatedAtAction(nameof(SessionUpsert), new { id = result.HorseID }, result);
            //}

            return Ok(result);
        }

    }
}
