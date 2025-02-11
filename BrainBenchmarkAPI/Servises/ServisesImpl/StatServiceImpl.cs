using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Servises.ServisesImpl
{
    public class StatServiceImpl : IStatService
    {
        private readonly DataContext _context;

        public StatServiceImpl(DataContext context)
        {
            _context = context;
        }

        public async Task<PlayerStatsModel> GetPlayerStats(Guid id)
        {
            var playerAttempts = await _context.Attempts
                .Include(at => at.Player)
                .Include(at => at.Game)
                .Where(at => at.Player.Id == id)
                .ToListAsync();

            var player = await _context.Users.FindAsync(id);

            //if (player == null) return NotFound(new ResponseModel("Error", "Can't find the player"));

            var attemptsCounter = playerAttempts.Count();

            //if (attemptsCounter == 0) return Ok(new ResponseModel("Success", "User has no attempts yet"));

            var averageAttemptsADay = attemptsCounter / (DateTime.Now.ToUniversalTime() - player.CreateTime).TotalDays;

            var groupsByDayOfTheWeek = playerAttempts
                .GroupBy(at => (int)at.AttemptDate.DayOfWeek)
                .Select(at => new { Day = at.Key, Count = at.Count() })
                .OrderByDescending(at => at.Count).ToList();

            var mostPopularDay = groupsByDayOfTheWeek[0].Day;
            DayOfTheWeek day = (DayOfTheWeek)Enum.GetValues<DayOfTheWeek>().GetValue(mostPopularDay - 1);

            var groupsByGames = playerAttempts
                .GroupBy(at => at.Game.Name)
                .Select(at => new { Game = at.Key, Count = at.Count() })
                .OrderByDescending(at => at.Count).ToList();
            var mostPopularGame = groupsByGames[0].Game;

            return new PlayerStatsModel(attemptsCounter, averageAttemptsADay, day, mostPopularGame);
        }

        public async Task<GameStatsModel> GetGameStats(Guid id)
        {
            var game = _context.Games.Find(id);

            //if (game == null) return NotFound(new ResponseModel("Error", "Can't find the game"));

            var gameAttempts = await _context.Attempts
                .Include(at => at.Game)
                .Where(at => at.Game.Id == id)
                .ToListAsync();

            int attemptsCounter = gameAttempts.Count();

            var gameResultStats = gameAttempts
                .GroupBy(at => at.Result)
                .Select(at => new { Res = at.Key, Percent = at.Count() / attemptsCounter })
                .ToList();

            Dictionary<int, double> results = new Dictionary<int, double>();
            // result : result amount / allAttemptsAmount

            foreach (var stat in gameResultStats)
            {
                results[stat.Res] = stat.Percent;
            }


            return new GameStatsModel(attemptsCounter, results);
        }

        public async Task<PlayerGameStatsModel> GetPlayerGameStats(Guid gameId, Guid playerId)
        {
            var gamePlayerAttempts = await _context.Attempts
                .Include(at => at.Game)
                .Include(at => at.Player)
                .Where(at => at.Game.Id == gameId && at.Player.Id == playerId)
                .OrderByDescending(at => at.Result)
                .ToListAsync();

            var game = await _context.Games.FindAsync(gameId);
            var player = await _context.Users.FindAsync(playerId);

            //if (game == null || player == null) return NotFound(new ResponseModel("Error", "Can't find player or game"));

            int attemptsCounter = gamePlayerAttempts.Count();
            //if (attemptsCounter == 0) return Ok(new ResponseModel("Success", "User has no attempts yet"));

            var allGameAttempts = _context.Attempts
                .Include(at => at.Game)
                .Include(at => at.Player)
                .Where(at => at.Game.Id == gameId);
            var averageGameRes = allGameAttempts.Average(at => at.Result);
            var gameAttemptsCounter = allGameAttempts.Count();


            var groupsByPlayers = allGameAttempts
                .GroupBy(at => at.Player.Name)
                .Select(at => new { Player = at.Key, Average = at.Average(attempt => attempt.Result) })
                .OrderByDescending(at => at.Average);


            allGameAttempts = allGameAttempts
                .Where(at => at.Player.Id == playerId);
            var averagePlayerGameRes = allGameAttempts.Average(at => at.Result);

            var averagePlayerBetterThen = groupsByPlayers
                .Where(at => at.Average < averagePlayerGameRes).Count();

            int bestRes = gamePlayerAttempts[0].Result;

            allGameAttempts = allGameAttempts
                .Where(at => at.Result < bestRes);
            var bestBetterThen = allGameAttempts.Count() / gameAttemptsCounter * 100;

            return new PlayerGameStatsModel(attemptsCounter, bestRes, bestBetterThen, averagePlayerGameRes, averagePlayerBetterThen);
        }
    }
}
