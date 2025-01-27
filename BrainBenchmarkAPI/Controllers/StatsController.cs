using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        /// <summary>
        /// Get stats from certain player that contains his attempts couner, average attempts a day, 
        /// favorite day of the week and favorite game
        /// </summary>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPlayerStats([Required, FromQuery] Guid id)
        {
            var playerAttempts = await _context.Attempts
                .Include(at => at.Player)
                .Where(at => at.Player.Id == id)
                .ToListAsync();

            var attemptsCounter = playerAttempts.Count();

            return Ok();
        }


        /// <summary>
        /// Get stats from certain game that contatins attempts counter and dict with game results and their percentage
        /// </summary>
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGameStats([Required, FromQuery] Guid id)
        {
            var gameAttempts = await _context.Attempts
                .Include(at => at.Game)
                .Where(at => at.Game.Id == id)
                .ToListAsync();

            int attemptsCounter = gameAttempts.Count();

            return Ok();
        }

        /// <summary>
        /// Get stats from certain player game stats that contains a bunch of different stats
        /// </summary>
        [HttpGet("game{gameId}/player{playerId}")]
        public async Task<IActionResult> GetPlayerGameStats([Required, FromQuery] Guid gameId, [Required, FromQuery] Guid playerId)
        {
            var gamePlayerAttempts = await _context.Attempts
                .Include(at => at.Game)
                .Include(at => at.Player)
                .Where(at => at.Game.Id == gameId && at.Player.Id == playerId)
                .ToListAsync();

            int attemptsCounter = gamePlayerAttempts.Count();

            return Ok();
        }
    }
}
