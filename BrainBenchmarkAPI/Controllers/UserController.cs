using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Data.Entities;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using BrainBenchmarkAPI.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Web.Helpers;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly TokenManager _tokenManager = new TokenManager();
        private readonly IUserServise _userServise;

        public UserController(DataContext dbContext, IUserServise userServise)
        {
            _context = dbContext;
            _userServise = userServise;
        }


        /// <summary>
        /// Register new user
        /// </summary>
        /// <response code="201">Returns the token of the newly created user</response>
        /// <response code="400">Invalid arguments</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel user) {
            //var checkEmailUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            //if (checkEmailUser != null) return BadRequest(new ResponseModel("Error", "This email is already used"));

            //var newUser = new UserDb(user);
            //_context.Users.Add(newUser);
            //await _context.SaveChangesAsync();

            //var token = _tokenManager.CreateTokenById(newUser.Id);

            //return Ok(new TokenResponseModel(token));

            return Ok(_userServise.RegisterUser(user));
        }


        /// <summary>
        /// Login user to the system
        /// </summary>
        /// <response code="200">Returns the token of the user</response>
        /// <response code="400">Invalid credentials (user not found)</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginModel user)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkUser == null || !Crypto.VerifyHashedPassword(checkUser.Password, user.Password)) 
                return BadRequest(new ResponseModel("Error", "Invalid credentials"));

            var token = _tokenManager.CreateTokenById(checkUser.Id);

            return Ok(new TokenResponseModel(token));
        }


        /// <summary>
        /// Get user profile
        /// </summary>
        /// <response code="200">Returns user profile</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        [HttpGet("profile")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound(new ResponseModel("Error", "User not found"));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            if (user == null) return NotFound(new ResponseModel("Error", "User not found"));

            return Ok(new UserModel(user));
        }


        /// <summary>
        /// Logout from the system
        /// </summary>
        /// <response code="200">Successfully logout</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("logout")]
        [Authorize]
        [CheckTokenFilter]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.GetTokenAsync("access_token").Result;

            var blackToken = new BlacklistTokenDb(token);
            _context.BlacklistTokens.Add(blackToken);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
