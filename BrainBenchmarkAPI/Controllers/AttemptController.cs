using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Data.Entities;
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
    [Route("api/attempts")]
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
        [HttpGet]
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
        [HttpGet("user")]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttemptInfo([Required] Guid id)
        {
            var attempt = await _context.Attempts.FindAsync(id);

            if (attempt == null) return NotFound(new ResponseModel("Error", "Can't find the attempt"));

            var resAttempt = new AttemptModel(attempt);

            return Ok(resAttempt);
        }


        /// <summary>
        /// Add attempt to favorites
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Can't find the attempt</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> SaveAttempt([Required] Guid id)
        {
            var attempt = await _context.Attempts
                .Include(at => at.Player)
                .FirstOrDefaultAsync(at => at.Id == id);

            if (attempt == null) return NotFound(new ResponseModel("Error", "Can't find the attempt"));

            var savedAttempt = new SavedAttemptDb(attempt.Id, attempt.Player);

            _context.SavedAttempts.Add(savedAttempt);

            await _context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Remove attempt from favorites
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Can't find the attempt</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> DeleteAttemptFromSaved([Required] Guid id)
        {
            var attempt = await _context.Attempts.FindAsync(id);

            if (attempt == null) return NotFound(new ResponseModel("Error", "Can't find the attempt"));

            _context.Attempts.Remove(attempt);

            await _context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Add attempt to db
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <param name="playerId">Id of the player</param>
        /// <param name="result">Result of the game</param>
        /// <param name="date">Attempt start time</param>
        /// <response code="201">Success</response>
        /// <response code="404">Can't find player or game</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(List<AttemptShortModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPost("game{gameId}/player{playerId}")]
        public async Task<IActionResult> AddAttempt([Required] Guid gameId, [Required] Guid playerId,
            [Required, FromQuery] int result, [Required, FromQuery] DateTime date)
        {
            var game = await _context.Games.FindAsync(gameId);
            var player = await _context.Users.FindAsync(playerId);

            if (game == null || player == null) return NotFound(new ResponseModel("Error", "Can't find game or player"));

            var attempt = new AttemptDb(player, game, result, date);

            _context.Attempts.Add(attempt);

            await _context.SaveChangesAsync();

            return Created();
        }


        [HttpGet("saved")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> GetSavedAttempts()
        {
            return Ok();
        }
    }
}
