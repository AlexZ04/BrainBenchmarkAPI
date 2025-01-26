namespace BrainBenchmarkAPI.Models
{
    public class PlayerGameStatsModel
    {
        public int AttemptsCounter { get; set; }
        public int BestResult { get; set; }
        public double BestBetterThen { get; set; }
        public double AverageResult { get; set; }
        public double AverageBetterThen { get; set; }
        List<AttemptShortModel> SavedAttempts { get; set; }

        public PlayerGameStatsModel(int attempts, int best, double bestBetterThen, double avgRes, double avgBetterThen, 
            List<AttemptShortModel> saved)
        {
            AttemptsCounter = attempts;
            BestResult = best;
            BestBetterThen = bestBetterThen;
            AverageResult = avgRes;
            AverageBetterThen = avgBetterThen;
            SavedAttempts = saved;
        }
    }
}
