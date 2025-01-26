namespace BrainBenchmarkAPI.Models
{
    public class PlayerStatsModel
    {
        public int AttemptsCounter { get; set; }
        public double AverageAttemptsADay { get; set; }
        public DayOfTheWeek DayOfTheWeek { get; set; }

        public PlayerStatsModel(int attempts, double average, DayOfTheWeek day)
        {
            AttemptsCounter = attempts;
            AverageAttemptsADay = average;
            DayOfTheWeek = day;
        }
    }
}
