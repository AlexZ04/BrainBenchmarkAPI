namespace BrainBenchmarkAPI.Models
{
    public class StatsModel
    {
        public int AttemptsCounter { get; set; }
        Dictionary<int, double> Results { get; set; } // ranged results : percentage result 

        public StatsModel(int attemptsCounter, Dictionary<int, double> results)
        {
            AttemptsCounter = attemptsCounter;
            Results = results;
        }
    }
}
