using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Data.Entities;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Servises.ServisesImpl
{
    public class UserServiseImpl : IUserServise
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public UserServiseImpl(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<TokenResponseModel> RegisterUser(UserRegisterModel user)
        {
            var checkEmailUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            //if (checkEmailUser != null) return BadRequest(new ResponseModel("Error", "This email is already used"));

            var newUser = new UserDb(user);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = _tokenService.CreateTokenById(newUser.Id);

            return new TokenResponseModel(token);
        }
    }
}
