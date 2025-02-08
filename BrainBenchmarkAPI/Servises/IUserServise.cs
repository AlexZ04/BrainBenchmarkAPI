using BrainBenchmarkAPI.Models;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Servises
{
    public interface IUserServise
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
        public Task<TokenResponseModel> LoginUser(UserLoginModel user);
        public Task<UserModel> Profile(ClaimsPrincipal user);
        public void Logout(string? token);
    }
}
