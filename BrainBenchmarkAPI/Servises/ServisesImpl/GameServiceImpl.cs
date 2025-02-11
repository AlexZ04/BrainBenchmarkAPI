using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Exceptions;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Servises.ServisesImpl
{
    public class GameServiceImpl : IGameService
    {
        private readonly DataContext _context;

        public GameServiceImpl(DataContext context) 
        {
            _context = context;
        }

        public async Task<List<GameShortModel>> GetAllGames()
        {
            var games = await _context.Games.ToListAsync();

            List<GameShortModel> result = new List<GameShortModel>();

            foreach (var game in games)
            {
                result.Add(new GameShortModel(game));
            }

            return result;
        }

        public async Task<GameModel> GetGameById(Guid id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.GAME_NOT_FOUND);

            return new GameModel(game);
        }
    }
}
