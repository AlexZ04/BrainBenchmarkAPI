namespace BrainBenchmarkAPI.Models
{
    public class PlayerStatsModel
    {
        public int AttemptsCounter { get; set; }
        public double AverageAttemptsADay { get; set; }
        public DayOfTheWeek DayOfTheWeek { get; set; }
        public string FavoriteGame { get; set; }

        public PlayerStatsModel(int attempts, double average, DayOfTheWeek day, string game)
        {
            AttemptsCounter = attempts;
            AverageAttemptsADay = average;
            DayOfTheWeek = day;
            FavoriteGame = game;
        }
    }
}
