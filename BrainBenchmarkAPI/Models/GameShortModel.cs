using BrainBenchmarkAPI.Data;

namespace BrainBenchmarkAPI.Models
{
    public class GameShortModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GameType Type { get; set; }

        public GameShortModel(GameDb game)
        {
            Id = game.Id;
            Name = game.Name;
            Type = game.Type;
        }
    }
}
