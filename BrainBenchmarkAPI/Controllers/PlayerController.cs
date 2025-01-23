using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> GetPlayerProfile([Required, FromQuery] Guid id)
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
    }
}
