using BrainBenchmarkAPI.Data.Entities;

namespace BrainBenchmarkAPI.Models
{
    public class PlayerShortModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public Role Role { get; set; }
        public int GamesPlayed { get; set; }

        public PlayerShortModel(UserDb user)
        {
            Id = user.Id;
            Name = user.Name;
            Gender = user.Gender;
            Birthday = user.Birthdate;
            Role = user.Role;
            GamesPlayed = user.GamesPlayed;
        }
    }
}
