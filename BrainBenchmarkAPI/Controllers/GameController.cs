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
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;

        public GameController(DataContext context)
        {
            _context = context;
        }


        [HttpPut("games")]
        public async Task<IActionResult> AddGame()
        {
            var game = new GameDb("Test");

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }


        /// <summary>
        /// Get list of short models of all games
        /// </summary>
        /// <response code="200">Returns list of short models of all games</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<GameShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("games")]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _context.Games.ToListAsync();

            List<GameShortModel> result = new List<GameShortModel>();

            foreach (var game in games)
            {
                result.Add(new GameShortModel(game));
            }

            return Ok(games);
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
        [HttpGet("game/{id}")]
        public async Task<IActionResult> GetGameById([Required] Guid id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null) return NotFound(new ResponseModel("Error", "Can't find game by id"));

            var gameModel = new GameModel(game);

            return Ok(gameModel);
        }
    }
}
