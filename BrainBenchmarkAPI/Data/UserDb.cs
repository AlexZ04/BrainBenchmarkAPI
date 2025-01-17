using BrainBenchmarkAPI.Models;

namespace BrainBenchmarkAPI.Data
{
    // user in database
    public class UserDb
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreateTime { get; set; }

        public UserDb() {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now.ToUniversalTime();
        }

        public UserDb(UserRegisterModel user)
        {

        }
    }
}
