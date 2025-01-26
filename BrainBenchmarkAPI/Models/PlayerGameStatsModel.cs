namespace BrainBenchmarkAPI.Models
{
    public class PlayerGameStatsModel
    {
        public int AttemptsCounter { get; set; }

        public PlayerGameStatsModel(int attempts)
        {
            AttemptsCounter = attempts;
        }
    }
}
