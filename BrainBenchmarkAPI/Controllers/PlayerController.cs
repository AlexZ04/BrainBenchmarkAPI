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
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetPlayerProfile([Required, FromQuery] Guid id)
        {
            var dbPlayer = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPlayer == null) return NotFound(new ResponseModel("Error", "Wrong user id!"));

            var player = new PlayerInfoModel(new UserModel(dbPlayer));

            return Ok(player);
        }
    }
}
