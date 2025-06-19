using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.TrainerAPI.ApplicationLayer;
using Services.TrainerAPI.Domain;
using Services.TrainerAPI.Domain.Contracts;
using Services.TrainerAPI.Domain.DTO;

namespace Services.TrainerAPI.Controllers
{
    [Route("api/trainer")]
    [ApiController]
    //[Authorize]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainers()
        {
            var response = await _trainerService.GetAllTrainersAsync();
            if (!response.IsSuccessful)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainerById(int id)
        {
            var response = await _trainerService.GetTrainerByIdAsync(id);
            if (!response.IsSuccessful)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerDTO trainer)
        {
            if (trainer == null)
            {
                return BadRequest("Trainer is null");
            }

            var response = await _trainerService.CreateTrainerAsync(trainer);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction(nameof(GetTrainerById), new { id = ((Trainer)response.Result).TrainerID }, response);
        }
    }
}
