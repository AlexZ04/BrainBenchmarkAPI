using BrainBenchmarkAPI.Models;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Servises
{
    public interface IAttemptService
    {
        public Task<List<AttemptShortModel>> GetLastAttempts();
        public Task<List<AttemptShortModel>> GetUserLastAttempts(ClaimsPrincipal user);
        public Task<AttemptModel> GetAttemptInfo(Guid id);
        public Task SaveAttempt(Guid id, ClaimsPrincipal user);
        public Task DeleteAttemptFromSaved(Guid id, ClaimsPrincipal user);
        public Task AddAttempt(Guid gameId, Guid playerId,
            int result, DateTime date);
        public Task<List<AttemptShortModel>> GetSavedAttempts(ClaimsPrincipal user);
    }
}
