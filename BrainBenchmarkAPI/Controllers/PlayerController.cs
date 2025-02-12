using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        /// <summary>
        /// Get list of all players
        /// </summary>
        /// <response code="200">Returns the list of all players</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<PlayerShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlayers()
        {
            return Ok(await _playerService.GetAllPlayers());
        }


        /// <summary>
        /// Get full player profile with stats
        /// </summary>
        /// <response code="200">Returns the info</response>
        /// <response code="404">Can't find the player</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(PlayerInfoModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetPlayerProfile([Required] Guid id)
        {
            return Ok(await _playerService.GetPlayerProfile(id));
        }


        /// <summary>
        /// Change player role. If player is in the admins list, you can't downgrade him
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="403">Player is in the admins list</response>
        /// <response code="404">Can't find the player</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPost("admin")]
        [Authorize]
        [CheckTokenFilter]
        [CheckAdminRoleFilter]
        public async Task<IActionResult> ChangeUserRole([Required, FromQuery] Guid id)
        {
            await _playerService.ChangeUserRole(id);

            return Ok();
        }
    }
}
