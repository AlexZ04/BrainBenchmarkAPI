using BrainBenchmarkAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly DataContext _context;

        public StatsController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPlayerStats([Required, FromQuery] Guid id)
        {
            return Ok();
        }


        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGameStats([Required, FromQuery] Guid id)
        {
            return Ok();
        }

        [HttpGet("game{gameId}/player{playerId}")]
        public async Task<IActionResult> GetPlayerGameStats([Required, FromQuery] Guid gameId, [Required, FromQuery] Guid playerId)
        {
            return Ok();
        }
    }
}
