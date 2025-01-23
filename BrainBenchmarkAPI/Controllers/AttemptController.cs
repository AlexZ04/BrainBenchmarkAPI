using BrainBenchmarkAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly DataContext _context;

        public AttemptController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("attempt/{id}")]
        public async Task<IActionResult> GetAttemptInfo([Required, FromQuery] Guid id)
        {
            return Ok();
        }


        [HttpPut("attempt/{id}")]
        public async Task<IActionResult> SaveAttempt([Required, FromQuery] Guid id)
        {
            return Ok();
        }


        [HttpDelete("attempt/{id}")]
        public async Task<IActionResult> DeleteAttemptFromSaved([Required, FromQuery] Guid id)
        {
            return Ok();
        }


        [HttpPost("attempt/game{gameId}/player{playerId}")]
        public async Task<IActionResult> AddAttempt([Required, FromQuery] Guid gameId, [Required, FromQuery] Guid playerId)
        {
            return Ok();
        }
    }
}
