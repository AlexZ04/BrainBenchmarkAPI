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
        /// Get stats from certain player that contains his attempts counter, average attempts a day, 
        /// favorite day of the week and favorite game
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Can't find the player</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(PlayerStatsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPlayerStats([Required] Guid id)
        {
            var playerAttempts = await _context.Attempts
                .Include(at => at.Player)
                .Include(at => at.Game)
                .Where(at => at.Player.Id == id)
                .ToListAsync();

            var player = await _context.Users.FindAsync(id);

            if (player == null) return NotFound(new ResponseModel("Error", "Can't find the player"));

            var attemptsCounter = playerAttempts.Count();

            if (attemptsCounter == 0) return Ok(new ResponseModel("Success", "User has no attempts yet"));

            var averageAttemptsADay = attemptsCounter / (DateTime.Now.ToUniversalTime() - player.CreateTime).TotalDays;

            var groupsByDayOfTheWeek = playerAttempts
                .GroupBy(at => (int) at.AttemptDate.DayOfWeek)
                .Select(at => new { Day = at.Key, Count = at.Count() })
                .OrderByDescending(at => at.Count).ToList();

            var mostPopularDay = groupsByDayOfTheWeek[0].Day;
            DayOfTheWeek day = (DayOfTheWeek)Enum.GetValues<DayOfTheWeek>().GetValue(mostPopularDay - 1);

            var groupsByGames = playerAttempts
                .GroupBy(at => at.Game.Name)
                .Select(at => new { Game = at.Key, Count = at.Count() })
                .OrderByDescending(at => at.Count).ToList();
            var mostPopularGame = groupsByGames[0].Game;

            return Ok(new PlayerStatsModel(attemptsCounter, averageAttemptsADay, day, mostPopularGame));
        }


        /// <summary>
        /// Get stats from certain game that contatins attempts counter and dict with game results and their percentage
        /// </summary>
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGameStats([Required] Guid id)
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
        public async Task<IActionResult> GetPlayerGameStats([Required] Guid gameId, [Required] Guid playerId)
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
