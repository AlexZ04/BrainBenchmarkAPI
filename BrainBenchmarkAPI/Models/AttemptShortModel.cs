using BrainBenchmarkAPI.Data.Entities;

namespace BrainBenchmarkAPI.Models
{
    public class AttemptShortModel
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public int Result { get; set; }
        public DateTime AttemptDate { get; set; }

        public AttemptShortModel(AttemptDb attempt)
        {
            Id = attempt.Id;
            GameName = attempt.Game.Name;
            Result = attempt.Result;
            AttemptDate = attempt.AttemptDate;
        }
    }
}
