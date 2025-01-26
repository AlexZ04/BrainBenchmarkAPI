namespace BrainBenchmarkAPI.Models
{
    public class GameStatsModel
    {
        public int AttemptsCounter { get; set; }
        Dictionary<int, double> Results { get; set; } // ranged results : percentage result 

        public GameStatsModel(int attemptsCounter, Dictionary<int, double> results)
        {
            AttemptsCounter = attemptsCounter;
            Results = results;
        }
    }
}
