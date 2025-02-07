using BrainBenchmarkAPI.Models;

namespace BrainBenchmarkAPI.Servises
{
    public interface IUserServise
    {
        public Task<TokenResponseModel> RegisterUser(UserRegisterModel user);
    }
}
