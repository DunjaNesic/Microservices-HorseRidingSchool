using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.HorseAPI.ApplicationLayer;
using Services.HorseAPI.Domain;
using Services.HorseAPI.Domain.DTO;

namespace Services.HorseAPI.Controllers
{
    [Route("api/horse")]
    [ApiController]
    //[Authorize]
    public class HorseController : ControllerBase
    {
        private readonly HorseService _horseService;

        public HorseController(HorseService horseService)
        {
            _horseService = horseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHorses()
        {
            var response = await _horseService.GetAllHorsesAsync();
            if (!response.IsSuccessful)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorseById(int id)
        {
            var response = await _horseService.GetHorseByIdAsync(id);
            if (!response.IsSuccessful)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateHorse([FromBody] CreateHorseDTO horse)
        {
            if (horse == null)
            {
                return BadRequest("Horse is null");
            }

            var response = await _horseService.CreateHorseAsync(horse);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction(nameof(GetHorseById), new { id = ((Horse)response.Result).HorseID }, response);
        }
    }
}
