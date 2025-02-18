﻿using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Exceptions;
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

            if (player == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_NOT_FOUND);

            var attemptsCounter = playerAttempts.Count();

            if (attemptsCounter == 0)
                throw new UserHasNoAttemptsException(ErrorTitles.USER_HAS_NO_ATTEMPS_EXCEPTION, ErrorMessages.USER_HAS_NO_ATTEMPS);

            var averageAttemptsADay = attemptsCounter / (DateTime.Now.ToUniversalTime() - player.CreateTime).TotalDays;

            var popularDayGroup = playerAttempts
                .GroupBy(a => (int)a.AttemptDate.DayOfWeek)
                .OrderByDescending(g => g.Count())
                .First();

            var day = (DayOfTheWeek)popularDayGroup.Key;

            var groupsByGames = playerAttempts
                .GroupBy(at => at.Game.Name)
                .Select(at => new { Game = at.Key, Count = at.Count() })
                .OrderByDescending(at => at.Count).ToList();
            var mostPopularGame = groupsByGames[0].Game;

            return new PlayerStatsModel(attemptsCounter, averageAttemptsADay, day, mostPopularGame);
        }

        public async Task<GameStatsModel> GetGameStats(Guid id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.GAME_NOT_FOUND);

            var gameAttempts = await _context.Attempts
                .Include(at => at.Game)
                .Where(at => at.Game.Id == id)
                .ToListAsync();

            int attemptsCounter = gameAttempts.Count();

            var gameResultStats = gameAttempts
                .GroupBy(at => at.Result)
                .Select(at => new { Res = at.Key, Percent = at.Count() / attemptsCounter })
                .ToList();

            // result : result amount / allAttemptsAmount

            Dictionary<int, double> results = gameAttempts
                .GroupBy(a => a.Result)
                .ToDictionary(
                    g => g.Key,
                    g => (double)g.Count() / attemptsCounter
                );

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

            if (game == null || player == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_OR_GAME_NOT_FOUND);

            int attemptsCounter = gamePlayerAttempts.Count();

            if (attemptsCounter == 0)
                throw new UserHasNoAttemptsException(ErrorTitles.USER_HAS_NO_ATTEMPS_EXCEPTION, ErrorMessages.USER_HAS_NO_ATTEMPS);

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

            int bestRes = gamePlayerAttempts.First().Result;

            allGameAttempts = allGameAttempts
                .Where(at => at.Result < bestRes);
            var bestBetterThen = allGameAttempts.Count() / gameAttemptsCounter * 100;

            return new PlayerGameStatsModel(attemptsCounter, bestRes, bestBetterThen, averagePlayerGameRes, averagePlayerBetterThen);
        }
    }
}
