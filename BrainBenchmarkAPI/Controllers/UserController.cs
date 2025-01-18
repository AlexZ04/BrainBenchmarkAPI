﻿using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Models;
using BrainBenchmarkAPI.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly TokenManager _tokenManager = new TokenManager();

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

            var token = _tokenManager.CreateTokenById(newUser.Id);

            return Ok(new TokenResponseModel(token));
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
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            if (checkUser == null) return BadRequest(new ResponseModel("Error", "Invalid credentials"));

            var token = _tokenManager.CreateTokenById(checkUser.Id);

            return Ok(new TokenResponseModel(token));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound(new ResponseModel("Error", "User not found"));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            if (user == null) return NotFound(new ResponseModel("Error", "User not found"));

            return Ok(new UserModel(user));
        }
    }
}
