namespace BrainBenchmarkAPI.Models
{
    public class PlayerInfoModel
    {
        public UserModel MainInfo { get; set; }
        public PlayerStatsModel PlayerStats { get; set; }
        
        public PlayerInfoModel(UserModel info, PlayerStatsModel playerStats) 
        { 
            MainInfo = info;
            PlayerStats = playerStats;
        }
    }
}
