using BrainBenchmarkAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BrainBenchmarkAPI.Models
{
    public class UserModel
    {
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Name { get; set; }
        public Role Role { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [AllowNull]
        public DateTime? Birthday { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime CreateTime { get; set; }
        public int GamesPlayed { get; set; }

        public UserModel(UserDb user)
        {
            Name = user.Name;
            Email = user.Email;
            Birthday = user.Birthdate;
            Gender = user.Gender;
            CreateTime = user.CreateTime;
            Role = user.Role;
            GamesPlayed = user.GamesPlayed;
        }
    }
}
