using BrainBenchmarkAPI.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BrainBenchmarkAPI.Models
{
    public class GameModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        public GameType Type { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Rules { get; set; }

        public GameModel(GameDb game)
        {
            Id = game.Id;
            Name = game.Name;
            Type = game.Type;
            Description = game.Description;
            Rules = game.Rules;
        }
    }
}
