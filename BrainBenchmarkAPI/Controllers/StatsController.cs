using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatService _statService;

        public StatsController(IStatService statService)
        {
            _statService = statService;
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
            return Ok(await _statService.GetPlayerStats(id));
        }


        /// <summary>
        /// Get stats from certain game that contatins attempts counter and dict with game results and their percentage
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Can't find the game</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(PlayerStatsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGameStats([Required] Guid id)
        {
            return Ok(await _statService.GetGameStats(id));
        }

        /// <summary>
        /// Get stats from certain player game stats that contains a bunch of different stats
        /// </summary>
        /// <response code="200">Get stats</response>
        /// <response code="404">Can't find player or game</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(PlayerGameStatsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("game{gameId}/player{playerId}")]
        public async Task<IActionResult> GetPlayerGameStats([Required] Guid gameId, [Required] Guid playerId)
        {
            return Ok(await _statService.GetPlayerGameStats(gameId, playerId));
        }
    }
}
