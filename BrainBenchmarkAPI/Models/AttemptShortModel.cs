using BrainBenchmarkAPI.Data;

namespace BrainBenchmarkAPI.Models
{
    public class AttemptShortModel
    {
        public string GameName { get; set; }
        public int Result { get; set; }
        public DateTime AttemptDate { get; set; }

        public AttemptShortModel(AttemptDb attempt)
        {
            GameName = attempt.Game.Name;
            Result = attempt.Result;
            AttemptDate = attempt.AttemptDate;
        }
    }
}
