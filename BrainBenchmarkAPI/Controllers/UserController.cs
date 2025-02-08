using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Filters;
using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Servises;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
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
            return Ok(await _userServise.RegisterUser(user));
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
            return Ok(await _userServise.LoginUser(user));
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
            return Ok(await _userServise.Profile(User));
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
        public IActionResult Logout()
        {
            _userServise.Logout(HttpContext.GetTokenAsync("access_token").Result);

            return Ok();
        }
    }
}
