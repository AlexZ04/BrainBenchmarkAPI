using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext dbContext)
        {
            _context = dbContext;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <response code="201">Returns the token of the newly created user</response>
        /// <response code="400">Invalid arguments</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel user) {
            var checkEmailUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkEmailUser != null) return BadRequest(new ResponseModel("Error", "This email is already used"));

            var newUser = new UserDb(user);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }
    }
}
