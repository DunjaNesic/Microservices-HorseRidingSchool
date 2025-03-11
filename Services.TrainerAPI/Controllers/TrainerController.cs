using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.TrainerAPI.ApplicationLayer;
using Services.TrainerAPI.Domain;

namespace Services.TrainerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly TrainerService _trainerService;

        public TrainerController(TrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetAllTrainers()
        {
            var trainers = await _trainerService.GetAllTrainersAsync();
            return Ok(trainers); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainerById(int id)
        {
            var trainer = await _trainerService.GetTrainerByIdAsync(id);
            if (trainer == null)
            {
                return NotFound(); 
            }
            return Ok(trainer);  
        }

        [HttpPost]
        public async Task<ActionResult> CreateTrainer([FromBody] Trainer trainer)
        {
            if (trainer == null)
            {
                return BadRequest("Trainer is null");
            }

            await _trainerService.CreateTrainerAsync(trainer);
            return CreatedAtAction(nameof(GetTrainerById), new { id = trainer.TrainerID }, trainer);  // Ako je uspešno kreiran, vrati 201 i lokaciju resursa
        }
    }
}
