using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/attempts")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly IAttemptService _attemptService;

        public AttemptController(IAttemptService attemptService)
        {
            _attemptService = attemptService;
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
            return Ok(await _attemptService.GetLastAttempts());
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
            return Ok(await _attemptService.GetUserLastAttempts(User));
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
            return Ok(await _attemptService.GetAttemptInfo(id));
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
            await _attemptService.SaveAttempt(id, User);

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
            await _attemptService.DeleteAttemptFromSaved(id, User);

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
            await _attemptService.AddAttempt(gameId, playerId, result, date);

            return Created();
        }

        /// <summary>
        /// Get list of short models of saved user attempts
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error with trying to find attempts</response>
        /// <response code="404">Can't find the user</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<AttemptShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("saved")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> GetSavedAttempts()
        {
            return Ok(await _attemptService.GetSavedAttempts(User));
        }
    }
}
