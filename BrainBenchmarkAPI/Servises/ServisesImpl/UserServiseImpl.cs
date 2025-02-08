using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Data.Entities;
using BrainBenchmarkAPI.Exceptions;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web.Helpers;

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

            if (checkEmailUser != null)
                throw new CredentialsException(ErrorMessages.USED_CREDENTIALS_EXCEPTION);

            var newUser = new UserDb(user);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = _tokenService.CreateTokenById(newUser.Id);

            return new TokenResponseModel(token);
        }

        public async Task<TokenResponseModel> LoginUser(UserLoginModel user)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (checkUser == null || !Crypto.VerifyHashedPassword(checkUser.Password, user.Password))
                throw new CredentialsException(ErrorMessages.INVALID_CREDENTIALS);

            var token = _tokenService.CreateTokenById(checkUser.Id);
            return new TokenResponseModel(token);
        }

        public async Task<UserModel> Profile(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null) return NotFound(new ResponseModel("Error", "User not found"));

            var userById = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            //if (userById == null) return NotFound(new ResponseModel("Error", "User not found"));

            return new UserModel(userById);
        }

        public async void Logout(string? token)
        {
            var blackToken = new BlacklistTokenDb(token);
            _context.BlacklistTokens.Add(blackToken);

            await _context.SaveChangesAsync();
        }
    }
}
