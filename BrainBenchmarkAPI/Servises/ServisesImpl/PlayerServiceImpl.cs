using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Data;
using BrainBenchmarkAPI.Exceptions;
using BrainBenchmarkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Servises.ServisesImpl
{
    public class PlayerServiceImpl : IPlayerService
    {
        private readonly DataContext _context;
        private readonly IStatService _statService;

        public PlayerServiceImpl(DataContext context, IStatService statService)
        {
            _context = context;
            _statService = statService;
        }

        public async Task<List<PlayerShortModel>> GetAllPlayers()
        {
            var players = await _context.Users.ToListAsync();

            List<PlayerShortModel> shortModels = new List<PlayerShortModel>();

            foreach (var p in players)
            {
                shortModels.Add(new PlayerShortModel(p));
            }

            return shortModels;
        }

        public async Task<PlayerInfoModel> GetPlayerProfile(Guid id)
        {
            var dbPlayer = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPlayer == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_NOT_FOUND);

            var playerStats = await _statService.GetPlayerStats(id);

            return new PlayerInfoModel(new UserModel(dbPlayer), playerStats);
        }

        public async Task ChangeUserRole(Guid id)
        {
            var dbPlayer = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPlayer == null)
                throw new SmthNotFoundException(ErrorTitles.NOT_FOUND_EXCEPTION, ErrorMessages.USER_NOT_FOUND);

            if (dbPlayer.Role == Role.User) dbPlayer.Role = Role.Admin;
            else
            {
                var admin = await _context.AdminList.FirstOrDefaultAsync(x => x.Id == id);

                if (admin != null)
                    throw new ForbidException(ErrorTitles.FORBID_EXCEPTION, ErrorMessages.DOWNGRADE_USER);

                dbPlayer.Role = Role.User;
            }

            await _context.SaveChangesAsync();
        }
    }
}
