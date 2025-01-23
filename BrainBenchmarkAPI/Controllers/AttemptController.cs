using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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


        /// <summary>
        /// Get last 10 attempts
        /// </summary>
        /// <response code="200">Get list of attempts short models</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(List<AttemptShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("attempt")]
        public async Task<IActionResult> GetLastAttempts()
        {
            var attempts = await _context.Attempts
                .OrderByDescending(at => at.AttemptDate)
                .Take(10)
                .ToListAsync();

            List<AttemptShortModel> res = new List<AttemptShortModel>();

            foreach (var attempt in attempts)
            {
                res.Add(new AttemptShortModel(attempt));
            }

            return Ok(res);
        }


        /// <summary>
        /// Get last 10 attempts of user
        /// </summary>
        /// <response code="200">Get list of user's attempts short models</response>
        /// <response code="404">Can't find the user</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(List<AttemptShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [HttpGet("attempt/user")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> GetUserLastAttempts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound(new ResponseModel("Error", "User not found"));

            var id = new Guid(userId);

            var attempts = await _context.Attempts
                .Include(at => at.Player)
                .OrderByDescending(at => at.AttemptDate)
                .Where(at => at.Player.Id == id)
                .Take(10)
                .ToListAsync();

            List<AttemptShortModel> res = new List<AttemptShortModel>();

            foreach (var attempt in attempts)
            {
                res.Add(new AttemptShortModel(attempt));
            }

            return Ok(res);
        }


        /// <summary>
        /// Get info about attempt
        /// </summary>
        /// <response code="200">Returns the info about the attempt</response>
        /// <response code="404">Can't find the attempt</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(AttemptModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("attempt/{id}")]
        public async Task<IActionResult> GetAttemptInfo([Required, FromQuery] Guid id)
        {
            var attempt = await _context.Attempts.FindAsync(id);

            if (attempt == null) return NotFound(new ResponseModel("Error", "Can't find the attempt"));

            var resAttempt = new AttemptModel(attempt);

            return Ok(resAttempt);
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
