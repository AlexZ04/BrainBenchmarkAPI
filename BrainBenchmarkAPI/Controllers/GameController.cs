using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }


        /// <summary>
        /// Get list of short models of all games
        /// </summary>
        /// <response code="200">Returns list of short models of all games</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<GameShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await _gameService.GetAllGames());
        }


        /// <summary>
        /// Get game model
        /// </summary>
        /// <response code="200">Returns the full game info</response>
        /// <response code="404">Can't find the game</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(GameModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById([Required] Guid id)
        {
            return Ok(await _gameService.GetGameById(id));
        }
    }
}
