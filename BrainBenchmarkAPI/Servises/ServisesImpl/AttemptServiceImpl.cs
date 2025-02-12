using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Data.Entities;
using BrainBenchmarkAPI.Exceptions;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Servises.ServisesImpl
{
    public class AttemptServiceImpl : IAttemptService
    {
        private readonly DataContext _context;

        public AttemptServiceImpl(DataContext context) 
        { 
            _context = context;
        }

        public async Task<List<AttemptShortModel>> GetLastAttempts()
        {
            var attempts = await _context.Attempts
                .OrderByDescending(at => at.AttemptDate)
                .Take(10)
                .ToListAsync();

            List<AttemptShortModel> res = new List<AttemptShortModel>();

            foreach (var attempt in attempts)
            {
                res.Add(new AttemptShortModel(attempt));
            }

            return res;
        }

        public async Task<List<AttemptShortModel>> GetUserLastAttempts(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_NOT_FOUND);

            var id = new Guid(userId);

            var attempts = await _context.Attempts
                .Include(at => at.Player)
                .OrderByDescending(at => at.AttemptDate)
                .Where(at => at.Player.Id == id)
                .Take(10)
                .ToListAsync();

            List<AttemptShortModel> res = new List<AttemptShortModel>();

            foreach (var attempt in attempts)
            {
                res.Add(new AttemptShortModel(attempt));
            }

            return res;
        }

        public async Task<AttemptModel> GetAttemptInfo(Guid id)
        {
            var attempt = await _context.Attempts.FindAsync(id);

            if (attempt == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.ATTEMPT_NOT_FOUND);

            var resAttempt = new AttemptModel(attempt);

            return resAttempt;
        }

        public async Task SaveAttempt(Guid id, ClaimsPrincipal user)
        {
            var attempt = await _context.Attempts
                .Include(at => at.Player)
                .FirstOrDefaultAsync(at => at.Id == id);

            if (attempt == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.ATTEMPT_NOT_FOUND);

            if (attempt.Player.Id != new Guid(user.FindFirst(ClaimTypes.NameIdentifier)?.Value))
                throw new ForbidException(ErrorTitles.FORBID_EXCEPTION, ErrorMessages.ACTION_WITH_OTHER_USER_ATTEMPT);

            var checkAttempt = await _context.SavedAttempts
                .FindAsync(id);
            if (checkAttempt != null)
                throw new ActionIsAlreadyDoneException(ErrorTitles.ACTION_IS_ALREADY_DONE_EXCEPTION, ErrorMessages.ATTEMPT_IS_ALREADY_SAVED);

            var savedAttempt = new SavedAttemptDb(attempt.Id, attempt.Player);

            _context.SavedAttempts.Add(savedAttempt);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttemptFromSaved(Guid id, ClaimsPrincipal user)
        {
            var attempt = await _context.SavedAttempts.FindAsync(id);

            if (attempt == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.ATTEMPT_NOT_FOUND);

            if (attempt.Player.Id != new Guid(user.FindFirst(ClaimTypes.NameIdentifier)?.Value))
                throw new ForbidException(ErrorTitles.FORBID_EXCEPTION, ErrorMessages.ACTION_WITH_OTHER_USER_ATTEMPT);

            _context.SavedAttempts.Remove(attempt);

            await _context.SaveChangesAsync();
        }

        public async Task AddAttempt(Guid gameId, Guid playerId, int result, DateTime date)
        {
            var game = await _context.Games.FindAsync(gameId);
            var player = await _context.Users.FindAsync(playerId);

            if (game == null || player == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_OR_GAME_NOT_FOUND);

            var attempt = new AttemptDb(player, game, result, date);

            _context.Attempts.Add(attempt);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AttemptShortModel>> GetSavedAttempts(ClaimsPrincipal user)
        {
            var userId = new Guid(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var savedAttempts = await _context.SavedAttempts
                .Include(at => at.Player)
                .Where(at => at.Player.Id == userId)
                .ToListAsync();

            List<AttemptShortModel> result = new List<AttemptShortModel>();
            
            foreach (var attempt in savedAttempts)
            {
                var fullAttemptModel = await _context.Attempts.FindAsync(attempt.Id);

                if (fullAttemptModel == null)
                    throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.ATTEMPT_NOT_FOUND);

                result.Add(new AttemptShortModel(fullAttemptModel));
            }

            return result;
        }
    }
}
