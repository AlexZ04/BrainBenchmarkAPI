using BrainBenchmarkAPI.Data;

namespace BrainBenchmarkAPI.Models
{
    public class AttemptModel
    {
        public string PlayerName { get; set; }
        public string GameName { get; set; }
        public int Result { get; set; }
        public DateTime AttemptDate { get; set; } // attempt start time

        public AttemptModel(AttemptDb attempt) 
        {
            PlayerName = attempt.Player.Name;
            GameName = attempt.Game.Name;
            Result = attempt.Result;
            AttemptDate = attempt.AttemptDate;
        }
    }
}
