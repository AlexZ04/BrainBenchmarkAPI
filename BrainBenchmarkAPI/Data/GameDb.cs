using BrainBenchmarkAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Data
{
    public class GameDb
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GameType Type { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public int SecondsAmount { get; set; }
        public int ErrorsAmount { get; set; }
        public List<AttemptDb> Attempts { get; set; }

        public GameDb() 
        {
            Id = Guid.NewGuid();
            Attempts = new List<AttemptDb>();
        }

        public GameDb(string name)
        {
            Id = Guid.NewGuid();
            Attempts = new List<AttemptDb>();
            Name = name;
            Type = GameType.ErrorAmounts;
            Description = " ";
            Rules = " ";
            SecondsAmount = 0;
            ErrorsAmount = 0;
        }
    }
}
