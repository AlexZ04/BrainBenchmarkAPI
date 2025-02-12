using BrainBenchmarkAPI.Models;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Servises
{
    public interface IPlayerService
    {
        public Task<List<PlayerShortModel>> GetAllPlayers();
        public Task<PlayerInfoModel> GetPlayerProfile(Guid id);
        public Task ChangeUserRole(Guid id);
    }
}
