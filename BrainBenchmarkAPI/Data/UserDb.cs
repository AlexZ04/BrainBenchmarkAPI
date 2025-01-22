using BrainBenchmarkAPI.Models;
using System.Web.Helpers;

namespace BrainBenchmarkAPI.Data
{
    // user in database
    public class UserDb
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now.ToUniversalTime();
        public Role Role { get; set; }
        public int GamesPlayed { get; set; }

        public UserDb() { }

        public UserDb(UserRegisterModel user)
        {
            Id = Guid.NewGuid();
            Name = user.Name;
            Password = Crypto.HashPassword(user.Password);
            Email = user.Email;
            Birthdate = user.Birthday;
            Gender = user.Gender;
            CreateTime = DateTime.Now.ToUniversalTime();
            Role = Role.User;
            GamesPlayed = 0;
        }
    }
}
