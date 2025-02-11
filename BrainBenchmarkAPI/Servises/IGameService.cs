using BrainBenchmarkAPI.Models;

namespace BrainBenchmarkAPI.Servises
{
    public interface IGameService
    {
        public Task<List<GameShortModel>> GetAllGames();
        public Task<GameModel> GetGameById(Guid id);
    }
}
