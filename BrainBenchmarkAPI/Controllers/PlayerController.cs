using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayerController(DataContext context)
        {
            _context = context;
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
            var players = await _context.Users.ToListAsync();

            List<PlayerShortModel> shortModels = new List<PlayerShortModel>();

            foreach (var p in players)
            {
                shortModels.Add(new PlayerShortModel(p));
            }

            return Ok(shortModels);
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
            var dbPlayer = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPlayer == null) return NotFound(new ResponseModel("Error", "Wrong user id!"));

            var player = new PlayerInfoModel(new UserModel(dbPlayer));

            return Ok(player);
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
            var dbPlayer = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPlayer == null) return NotFound(new ResponseModel("Error", "Wrong user id!"));

            if (dbPlayer.Role == Role.User) dbPlayer.Role = Role.Admin;
            else
            {
                var admin = await _context.AdminList.FirstOrDefaultAsync(x => x.Id == id);

                if (admin != null) return Forbid("You can't downgrade this user!");

                dbPlayer.Role = Role.User;
            }

            await _context.SaveChangesAsync();

            return Ok();
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound(new ResponseModel("Error", "User not found"));

            var id = new Guid(userId);

            var attemptsId = await _context.SavedAttempts
                .Include(at => at.Player)
                .Where(at => at.Player.Id == id)
                .ToListAsync();

            List<AttemptShortModel> res = new List<AttemptShortModel>();

            foreach (var attId in attemptsId)
            {
                var currentAttempt = await _context.Attempts.FindAsync(attId);

                if (currentAttempt == null) return BadRequest(new ResponseModel("Error", "Error with finding attempts!"));

                res.Add(new AttemptShortModel(currentAttempt));
            }

            return Ok(res);
        }
    }
}
