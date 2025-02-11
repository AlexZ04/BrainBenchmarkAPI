using BrainBenchmarkAPI.Models;

namespace BrainBenchmarkAPI.Servises
{
    public interface IStatService
    {
        public Task<PlayerStatsModel> GetPlayerStats(Guid id);
        public Task<GameStatsModel> GetGameStats(Guid id);
        public Task<PlayerGameStatsModel> GetPlayerGameStats(Guid gameId, Guid playerId);
    }
}
