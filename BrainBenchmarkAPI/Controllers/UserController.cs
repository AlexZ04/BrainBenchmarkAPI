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
        private readonly DbContext _dbContext;

        public UserController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Get() {
            //var user = await _dbContext.Users.ToListAsync();

            return Ok();
        }
    }
}
